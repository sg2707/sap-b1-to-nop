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


namespace NopAPIConnect
{
    public class NopAPIConnect : INopAPIConnect
    {
        private readonly ILog _logger;

        private readonly IConfigSettings _configService;
        public static HttpClient client { get; set; }
        public HttpResponseMessage response { get; set; }
        public HttpResponseMessage mfgresponse { get; set; }
        public HttpResponseMessage catgresponse { get; set; }
        public List<NOPCommerceApiManufactures> ManufactureIds { get; set; }
        public List<NopCommerceApiCategory> CategoryIds { get; set; }
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
            response = await client.GetAsync("api/token?username="+ nOPUserID + "&password="+ nOPPass + "");
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
            _logger.Info("Manufacturers Api started");
            mfgresponse = await client.GetAsync("api/manufacturers");
            if (mfgresponse.IsSuccessStatusCode)
            {
                var mnfresponse = mfgresponse.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<RootObject>(mnfresponse);
                ManufactureIds = result.manufacturers.ToList();
            }
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
        public async Task SaveProductsAsync(List<NOPCommerceApiProduct> products)
        {
            string sku = null;
            int id = 0;
            await GetManufacturerList();
            _logger.Info("Retrived rows from manufacturers Api");
            await GetCategoryList();
            _logger.Info("Retrived rows from categories Api");
            foreach (var product in products)
            {
                product.manufacturer_ids = ManufactureIds.Where(p => p.name == product.manufacturer).Select(p => p.id).ToList();
                product.category_ids = CategoryIds.Where(p => p.meta_keywords == product.category).Select(p => p.id).ToList();
                _logger.Info("Product Api started");
                response = await client.GetAsync("api/products?sku=" + product.sku);
                _logger.Info("Product Api retrieved rows by sku ");
                if (response.IsSuccessStatusCode)
                {
                    _logger.Info("Product Api response success");
                    try
                    {
                        var prodresponse = response.Content.ReadAsStringAsync().Result;
                        dynamic newproducts = JsonConvert.DeserializeObject(prodresponse);
                        sku = newproducts.products[0].sku;
                        id = newproducts.products[0].id;

                    }
                    catch { }
                }
                if (sku == null)
                {
                    var output = "{  \"product\": " + JsonConvert.SerializeObject(product) + "}";
                    var stringContent = new StringContent(output);
                    response = await client.PostAsync("api/products", stringContent);
                    _logger.Info("Api posted product to Nop");
                }
                else
                {
                    product.manufacturer_ids =null;
                    product.category_ids = null;
                    var output = "{  \"product\": " + JsonConvert.SerializeObject(product) + "}";
                    var stringContent = new StringContent(output);
                    response = await client.PutAsync("api/products/" + id, stringContent); //Mfg & catg
                    _logger.Info("Api updated product to Nop");
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
            int id = 0;
            foreach (var category in categories)
            {
                _logger.Info("Category Api started");
                response = await client.GetAsync("api/categories?metakey=" + category.meta_keywords);
                _logger.Info("Category Api retrieved rows by meta keywords ");
                if (response.IsSuccessStatusCode)
                {
                    _logger.Info("Category Api response success");
                    try
                    {
                        var ctgresponse = response.Content.ReadAsStringAsync().Result;
                        dynamic newctg = JsonConvert.DeserializeObject(ctgresponse);
                        metakey = newctg.categories[0].meta_keywords;
                        id = newctg.categories[0].id;

                    }
                    catch { }
                }
                var output = "{  \"category\": " + JsonConvert.SerializeObject(category) + "}";
                var stringContent = new StringContent(output);
                if (metakey == null)
                {
                    response = await client.PostAsync("api/categories", stringContent);
                    _logger.Info("Api posted category to Nop");
                }
                else
                {
                    response = await client.PutAsync("api/categories/" + id, stringContent); 
                    _logger.Info("Api updated category to Nop");
                }
            }
        }

        /// <summary>
        /// Save manufactures to nopComerce
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
                response = await client.GetAsync("api/categories?name=" + manufacturer.name);
                _logger.Info("Manufacturer Api retrieved rows by name ");
                if (response.IsSuccessStatusCode)
                {
                    _logger.Info("Manufacturer Api response success");
                    try
                    {
                        var mnfresponse = response.Content.ReadAsStringAsync().Result;
                        dynamic newmanuf = JsonConvert.DeserializeObject(mnfresponse);
                        name = newmanuf.manufacturers[0].name;
                        id = newmanuf.manufacturers[0].id;

                    }
                    catch { }
                }
                var output = "{  \"manufacturer\": " + JsonConvert.SerializeObject(manufacturer) + "}";
                var stringContent = new StringContent(output);
                if (name == null)
                {
                    response = await client.PostAsync("api/manufacturers", stringContent);
                    _logger.Info("Api posted manufacturer to Nop");
                }
                else
                {
                    response = await client.PutAsync("api/manufacturers/" + id, stringContent);
                    _logger.Info("Api updated product to Nop");
                }
            }
        }

        /// <summary>
        /// Gets order status by payment status
        /// </summary>
        /// <returns>Order status</returns>
        public async Task<List<NopCommerceApiOrder>>GetOrdersAsync()
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