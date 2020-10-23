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
        public string AccessToken { get; set; }

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
             GetAccessToken(_configService.NOPUserID, _configService.NOPPass, _configService.NOP_API_URL);
             //login
        }
        private async Task  GetAccessToken(string nOPUserID, string nOPPass,string noPBaseAdd)
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
            //call and get accesstoken
        }


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
        //here implement service
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
                var output = "{  \"product\": " + JsonConvert.SerializeObject(product) + "}";
                var stringContent = new StringContent(output);

                if (sku == null)
                {
                    response = await client.PostAsync("api/products", stringContent);
                    _logger.Info("Api posted product to Nop");
                }
                else
                {
                    response = await client.PutAsync("api/products/" + id, stringContent); //Mfg & catg
                    _logger.Info("Api updated product to Nop");
                }
            }
        }

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