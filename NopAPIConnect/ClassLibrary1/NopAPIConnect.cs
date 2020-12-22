using NopAPIConnect.Models;
using SAPData.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using log4net;
using Newtonsoft.Json.Linq;
using SAPData.Models;
using SAPData;
using System.Data.SqlClient;
using Utilities;

namespace NopAPIConnect
{
    public class NopAPIConnect : INopAPIConnect
    {


        private readonly ILog _logger;

        private readonly IConfigSettings _configService;
        public static HttpClient client { get; set; }
        private HttpResponseMessage response { get; set; }
        private HttpResponseMessage mfgresponse { get; set; }
        private HttpResponseMessage mfgcountresponse { get; set; }
        private HttpResponseMessage catgresponse { get; set; }
        private List<NOPCommerceApiManufactures> ManufactureIds { get; set; }
        private List<NopCommerceApiCategory> CategoryIds { get; set; }
        public NopAPIConnect(IConfigSettings configservice, ILog logger)
        {
            _logger = logger;
            _configService = configservice;
            client = new HttpClient();
            response = new HttpResponseMessage();
            client.BaseAddress = new Uri(configservice.NOP_API_URL);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            GetAccessToken(_configService.NOPUserID, _configService.NOPPass).ToString();
        }

        /// <summary>
        /// Gets access token
        /// </summary>
        /// <param name="nOPUserID">username</param>
        /// <param name="nOPPass">password</param>
        /// <param name="noPBaseAdd">url</param>
        /// <returns></returns>
        private async Task GetAccessToken(string nOPUserID, string nOPPass)
        {
            response = await client.GetAsync("api/token?username=" + nOPUserID + "&password=" + nOPPass + "");
            string accesstoken = null;
            if (response.IsSuccessStatusCode)
            {
                var tokresponse = response.Content.ReadAsStringAsync().Result;
                dynamic token = JsonConvert.DeserializeObject(tokresponse);
                accesstoken = token.access_token;
            }
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);
        }

        /// <summary>
        /// Gets manufacturer list
        /// </summary>
        /// <returns></returns>
        private async Task GetManufacturerList()
        {
            decimal totalrecords = 0;
            decimal totalpages = 0;
            List<NOPCommerceApiManufactures> results = new List<NOPCommerceApiManufactures>();
            mfgcountresponse = await client.GetAsync("api/manufacturers/count");
            if (mfgcountresponse.IsSuccessStatusCode)
            {
                var count = mfgcountresponse.Content.ReadAsStringAsync().Result;
                dynamic newcount = JsonConvert.DeserializeObject(count);
                totalrecords = newcount.count;
            }
            totalpages = totalrecords / 250;
            totalpages = Math.Ceiling(totalpages);
            _logger.Info("Manufacturers Api started");
            for (int page = 1; page <= totalpages; page++)
            {
                mfgresponse = await client.GetAsync("api/manufacturers?page=" + page);
                if (mfgresponse.IsSuccessStatusCode)
                {

                    var mnfresponse = mfgresponse.Content.ReadAsStringAsync().Result;
                    var list = JsonConvert.DeserializeObject<RootObject>(mnfresponse);
                    results.AddRange(list.manufacturers.ToList());

                }
            }
            ManufactureIds = results;
        }

        /// <summary>
        /// Gets category list
        /// </summary>
        /// <returns></returns>
        private async Task GetCategoryList()
        {
            _logger.Info("Categories Api started");
            catgresponse = await client.GetAsync("api/categories");
            if (catgresponse.IsSuccessStatusCode)
            {
                var ctgresponse = catgresponse.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<RootObjectCtg>(ctgresponse);
                CategoryIds = result.categories.ToList();
            }
        }

        /// <summary>
        /// Save products to nopCommerce
        /// </summary>
        /// <param name="products">Product list</param>
        /// <returns></returns>
        public async Task SaveProductsAsync(List<NOPCommerceApiProduct> products, ProgressBinder progress)
        {
            string sku = null;
            string attTypeDesc = null;
            int id = 0;
            SpecificationAttributeService _specificationAttributeService = new SpecificationAttributeService();
            HttpResponseMessage attresponse = new HttpResponseMessage();
            HttpResponseMessage postattresponse = new HttpResponseMessage();
            HttpResponseMessage specprodresponse = new HttpResponseMessage();
            await GetManufacturerList();
            _logger.Info("Retrived rows from manufacturers Api");
            await GetCategoryList();
            _logger.Info("Retrived rows from categories Api");
            foreach (var product in products)
            {
                if (progress.CancellationToken.IsCancellationRequested)
                    throw new Exception("User cancelled.");

                product.manufacturer_ids = ManufactureIds.Where(p => p.name == product.manufacturer).Select(p => p.id).ToList();
                product.category_ids = CategoryIds.Where(p => p.meta_keywords == product.category).Select(p => p.id).ToList();
                _logger.Info("Product Api started");

                //Load option spect by name
                response = await client.GetAsync("api/products?sku=" + product.sku);
                _logger.Info("Product Api retrieved rows by sku ");
                if (response.IsSuccessStatusCode)
                {
                    _logger.Info("Product Api response success");
                    var prodresponse = response.Content.ReadAsStringAsync().Result;
                    if ("{\"products\":[]}" != JObject.Parse(prodresponse).ToString(Newtonsoft.Json.Formatting.None).Trim())
                    {
                        _logger.Info("Product record present next process update record to NOP");
                        dynamic newproducts = JsonConvert.DeserializeObject(prodresponse);
                        sku = newproducts.products[0].sku;
                        id = newproducts.products[0].id;
                    }
                    else
                    {
                        _logger.Info("Product record not present next process post record to NOP");
                        sku = null;
                        id = 0;
                    }
                }
                var output = "{  \"product\": " + JsonConvert.SerializeObject(product) + "}";
                var stringContent = new StringContent(output);
                if (sku == null)
                {
                    response = await client.PostAsync("api/products", stringContent);
                    _logger.Info("Api posted product record to Nop");
                }
                else
                {
                    response = await client.PutAsync("api/products/" + id, stringContent); //Mfg & catg
                    _logger.Info("Api updated product record to Nop");
                }
                var specAttbts = _specificationAttributeService.GetSpecificationAttributeListBySku(product.sku).ToList();
                _logger.Info("Retrieve (" + specAttbts.Count + ") specification attributes by (" + product.sku + " ) from SAP");

                /////-------------------
                if (specAttbts.Count > 0)
                {
                    specprodresponse = await client.GetAsync("api/productspecificationattributes?product_id=" + id);
                    _logger.Info("Specification attribute mapping Api retrieved rows by product id ");
                    if (specprodresponse.IsSuccessStatusCode)
                    {
                        _logger.Info("Specification attribute Api response success");
                        var specprod = specprodresponse.Content.ReadAsStringAsync().Result;
                        if ("{\"product_specification_attributes\":[]}" != JObject.Parse(specprod).ToString(Newtonsoft.Json.Formatting.None).Trim())
                        {
                            var specprodmap = JsonConvert.DeserializeObject<RootObjectProductSpecMap>(specprod);
                            var listSpecs = specprodmap.product_specification_attributes;
                            if (id > 0)
                            {
                                foreach (var listSpec in listSpecs)
                                {
                                    await client.DeleteAsync("api/productspecificationattributes/" + listSpec.id);
                                    _logger.Info("deleted existing specification attribute mapping by id  " + listSpec.id + ", product Id " + id);
                                }
                            }
                        }

                        ///----------------------------------
                        foreach (var attbt in specAttbts)
                        {
                            if (attbt.control_type == 2)
                                attTypeDesc = "Option";
                            if (attbt.control_type == 1)
                                attTypeDesc = "CustomText";
                            attresponse = await client.GetAsync("api/specificationattributes?name=" + attbt.name);
                            _logger.Info("Retrieve specification attribute by " + attbt.name + " from NOP API");

                            if (attresponse.IsSuccessStatusCode)
                            {
                                var specresponse = attresponse.Content.ReadAsStringAsync().Result;
                                if ("{\"specification_attributes\":[]}" != JObject.Parse(specresponse).ToString(Newtonsoft.Json.Formatting.None).Trim())
                                {
                                    var result = JsonConvert.DeserializeObject<RootObjectSpec>(specresponse);
                                    var specAttOptions = result.specification_attributes[0].specification_attribute_options.Where(a => a.name == attbt.option_name);
                                    foreach (var specAttOption in specAttOptions)
                                    {
                                        var productSpecification = new List<NOPCommerceApiProductSpecification>
                                    { new NOPCommerceApiProductSpecification { product_id = id, specification_attribute_option_id = specAttOption.id
                                    , allow_filtering=true, show_on_product_page=true,display_order=0, attribute_type=attTypeDesc } };
                                        var spoutput = "{  \"product_specification_attribute\": " + JsonConvert.SerializeObject(productSpecification) + "}";
                                        var stringContentspec = new StringContent(spoutput.Replace("[", "").Replace("]", ""));
                                        postattresponse = await client.PostAsync("api/productspecificationattributes", stringContentspec);
                                        _logger.Info("Post specification attribute mapping details to NOP");

                                    }
                                }
                            }

                        }
                    }
                }
                
            }

        }

        /// <summary>
        /// Save categories to nopComerce
        /// </summary>
        /// <param name="categories">Category list</param>
        /// <returns></returns>
        public async Task SaveCategoriesAsync(List<NopCommerceApiCategory> categories)
        {
            string metakey = null;
            StringContent stringContent = null;
            int id = 0;
            int pid = 0;
            HttpResponseMessage paresponse = new HttpResponseMessage();
            foreach (var category in categories)
            {
                _logger.Info("Category Api started");
                response = await client.GetAsync("api/categories?meta_keywords=" + category.meta_keywords);
                _logger.Info("Category Api retrieved rows by meta keywords ");
                if (response.IsSuccessStatusCode)
                {
                    _logger.Info("Category Api response success");
                    var ctgresponse = response.Content.ReadAsStringAsync().Result;
                    if ("{\"categories\":[]}" != JObject.Parse(ctgresponse).ToString(Newtonsoft.Json.Formatting.None).Trim())
                    {
                        _logger.Info("Category record present next process update record to NOP");
                        dynamic newctg = JsonConvert.DeserializeObject(ctgresponse);
                        metakey = newctg.categories[0].meta_keywords;
                        id = newctg.categories[0].id;
                    }
                    else
                    {
                        _logger.Info("Category record not present next process post record to NOP");
                        metakey = null;
                        id = 0;
                    }
                }
                if (category.parent_meta_keywords != null)
                {
                    paresponse = await client.GetAsync("api/categories?meta_keywords=" + category.parent_meta_keywords);
                    _logger.Info("Sub category Api retrieved rows by meta keywords ");
                    if (paresponse.IsSuccessStatusCode)
                    {
                        var ctgptresponse = paresponse.Content.ReadAsStringAsync().Result;
                        if ("{\"categories\":[]}" != JObject.Parse(ctgptresponse).ToString(Newtonsoft.Json.Formatting.None).Trim())
                        {
                            _logger.Info("Category record present next process update record to NOP");
                            dynamic newptctg = JsonConvert.DeserializeObject(ctgptresponse);
                            pid = newptctg.categories[0].id;
                        }
                    }
                    var listCatg =
                    new NopCommerceApiCategory() { name = category.name, meta_keywords = category.meta_keywords, parent_category_id = pid };
                    var output = "{  \"category\": " + JsonConvert.SerializeObject(listCatg) + "}";
                    stringContent = new StringContent(output);
                }
                else
                {
                    var output = "{  \"category\": " + JsonConvert.SerializeObject(category) + "}";
                    stringContent = new StringContent(output);
                }

                if (metakey == null)
                {
                    response = await client.PostAsync("api/categories", stringContent);
                    _logger.Info("Api posted category record to Nop");
                }
                else
                {
                    response = await client.PutAsync("api/categories/" + id, stringContent);
                    _logger.Info("Api updated category record to Nop");
                }
            }
        }

        /// <summary>
        /// Save manufactures to nopCommerce
        /// </summary>
        /// <param name="manufacturers">Manufacturer List</param>
        /// <returns></returns>
        public async Task SaveManufacturesAsync(List<NOPCommerceApiManufactures> manufacturers)
        {
            string name = null;
            int id = 0;
            foreach (var manufacturer in manufacturers)
            {
                _logger.Info("Manufacturer Api started");
                response = await client.GetAsync("api/manufacturers?name=" + manufacturer.name);
                _logger.Info("Manufacturer Api retrieved rows by name ");
                if (response.IsSuccessStatusCode)
                {
                    _logger.Info("Manufacturer Api response success");
                    var mnfresponse = response.Content.ReadAsStringAsync().Result;
                    if ("{\"manufacturers\":[]}" != JObject.Parse(mnfresponse).ToString(Newtonsoft.Json.Formatting.None).Trim())
                    {
                        _logger.Info("Manufacturer record not present next process update record to NOP");
                        dynamic newmanuf = JsonConvert.DeserializeObject(mnfresponse);
                        name = newmanuf.manufacturers[0].name;
                        id = newmanuf.manufacturers[0].id;
                    }
                    else
                    {
                        _logger.Info("Manufacturer record not present next process post record to NOP");
                        name = null;
                        id = 0;
                    }
                }
                var output = "{  \"manufacturer\": " + JsonConvert.SerializeObject(manufacturer) + "}";
                var stringContent = new StringContent(output);
                if (name == null)
                {
                    response = await client.PostAsync("api/manufacturers", stringContent);
                    _logger.Info("Api posted manufacturer record to Nop");
                }
                else
                {
                    response = await client.PutAsync("api/manufacturers/" + id, stringContent);
                    _logger.Info("Api updated manufacturer record to Nop");
                }
            }
        }

        /// <summary>
        /// Save specification attribute to nopComerce
        /// </summary>
        /// <param name="Specattributes">specification attribute List</param>
        /// <returns></returns>
        public async Task SaveSpecificationAttributeAsync(List<NOPCommerceApiSpecificationAttribute> Specattributes)
        {
            string name = null;
            int id = 0;
            SpecificationAttributeService _specificationAttributeService = new SpecificationAttributeService();
            foreach (var specAttribute in Specattributes)
            {
                // list spec attribute
                _logger.Info("Specification attribute Api started");
                response = await client.GetAsync("api/specificationattributes?name=" + specAttribute.name);
                _logger.Info("Specification attribute Api retrieved rows by name ");
                if (response.IsSuccessStatusCode)
                {
                    _logger.Info("Specification attribute Api response success");
                    var specresponse = response.Content.ReadAsStringAsync().Result;
                    if ("{\"specification_attributes\":[]}" != JObject.Parse(specresponse).ToString(Newtonsoft.Json.Formatting.None).Trim())
                    {
                        dynamic newspec = JsonConvert.DeserializeObject(specresponse);
                        name = newspec.specification_attributes[0].name;
                        id = newspec.specification_attributes[0].id;
                    }
                    else
                    {
                        name = null;
                        id = 0;
                    }
                }
                if (specAttribute.control_type == 2)
                    specAttribute.specification_attribute_options = _specificationAttributeService.GetSpecificationOptionsList(specAttribute.attribute_id).ToList();
                else if (specAttribute.control_type == 1)
                    specAttribute.specification_attribute_options = new List<SpecificationAttributeOptions> { new SpecificationAttributeOptions { name = specAttribute.name, display_order = 0, specification_attribute_id = id } };
                var output = "{  \"specification_attribute\": " + JsonConvert.SerializeObject(specAttribute) + "}";
                var stringContent = new StringContent(output.Replace("\"id\":0,", ""));
                if (name == null)
                {
                    response = await client.PostAsync("api/specificationattributes", stringContent);
                    _logger.Info("Api posted specification attribute to Nop");
                }
                else
                {
                    response = await client.PutAsync("api/specificationattributes/" + id, stringContent);
                    _logger.Info("Api updated specification attribute to Nop");
                }
            }
        }

        public void SaveVehicle(List<NOPCommerceApiVehicle> vehicles)
        {
            foreach (var vehicle in vehicles)
            {
                SAPData.Models.DBContext dc = new SAPData.Models.DBContext();
                if (!dc.Database.Exists())
                    dc.Database.Connection.Open();
                dc.Database.CommandTimeout = 120;
                dc.Database.ExecuteSqlCommand(
                "exec  SI_SaveNopCommerceVehicle @Make,@ModelNo,@ModelName,@VehicleChassisGrp, @Chassis, @VehicleEngineGrp, @Engine, @CCPrefix, @CC, @CCSufix, @HandDrive, @TransmissionType, @TransmissionCode, @FuelType,@CountryOfManufacture, @ManufactureStart, @ManufactureEnd, @LastModifiedBy, @LastModifiedDate, @SAPVehicleId ",
                   new SqlParameter("@Make", vehicle.Make ?? DBNull.Value.ToString()),
                   new SqlParameter("@ModelNo", vehicle.ModelNo ?? DBNull.Value.ToString()),
                   new SqlParameter("@ModelName", vehicle.ModelName ?? DBNull.Value.ToString()),
                   new SqlParameter("@VehicleChassisGrp", vehicle.VehicleChassisGrp ?? DBNull.Value.ToString()),
                   new SqlParameter("@Chassis", vehicle.Chassis ?? DBNull.Value.ToString()),
                   new SqlParameter("@VehicleEngineGrp", vehicle.VehicleEngineGrp ?? DBNull.Value.ToString()),
                   new SqlParameter("@Engine", vehicle.Engine ?? DBNull.Value.ToString()),
                   new SqlParameter("@CCPrefix", vehicle.CCPrefix ?? DBNull.Value.ToString()),
                   new SqlParameter("@CC", vehicle.CC ?? DBNull.Value.ToString()),
                   new SqlParameter("@CCSufix", vehicle.CCSufix ?? DBNull.Value.ToString()),
                   new SqlParameter("@HandDrive", vehicle.HandDrive ?? DBNull.Value.ToString()),
                   new SqlParameter("@TransmissionType", vehicle.TransmissionType ?? DBNull.Value.ToString()),
                   new SqlParameter("@TransmissionCode", vehicle.TransmissionCode ?? DBNull.Value.ToString()),
                   new SqlParameter("@FuelType", vehicle.FuelType ?? DBNull.Value.ToString()),
                   new SqlParameter("@CountryOfManufacture", vehicle.CountryOfManufacture ?? DBNull.Value.ToString()),
                   new SqlParameter("@ManufactureStart", vehicle.ManufactureStart),
                   new SqlParameter("@ManufactureEnd", vehicle.ManufactureEnd),
                   new SqlParameter("@LastModifiedBy", vehicle.LastModifiedBy ?? DBNull.Value.ToString()),
                   new SqlParameter("@LastModifiedDate", vehicle.LastModifiedDate),
                   new SqlParameter("@SAPVehicleId", vehicle.SAPVehicleId));
            }
        }
        /// <summary>
        /// Gets order status by payment status
        /// </summary>
        /// <returns>Order status</returns>
        public async Task<List<NopCommerceApiOrder>> GetOrdersAsync()
        {
            dynamic neworder = null;
            response = await client.GetAsync("api/orders?payment_status=Paid&paid_at_min=2020-10-05T16:15:47-04:00");
            if (response.IsSuccessStatusCode)
            {
                var prodresponse = response.Content.ReadAsStringAsync().Result;
                neworder = JsonConvert.DeserializeObject(prodresponse);
            }
            return neworder;
        }
    }
}