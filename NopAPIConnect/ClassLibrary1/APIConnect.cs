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
        public NopAPIConnect(IConfigSettings configservice, ILog logger)
        {
            _logger = logger;
            _configService = configservice;
            client = new HttpClient();
            response = new HttpResponseMessage();
            client.BaseAddress = new Uri(configservice.NOP_API_URL);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            getAccessToken(_configService.NOPUserID, _configService.NOPPass, _configService.NOP_API_URL);
             //login
        }
        private async Task  getAccessToken(string nOPUserID, string nOPPass,string noPBaseAdd)
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

        //here implement service
        public async Task SaveProductsAsync(List<NOPCommerceApiProduct> products)
        {
            string sku = null;
            int id = 0;
            foreach (var product in products)
            {
                _logger.Info("Api started");
                response = await client.GetAsync("api/products?sku=" + product.sku);
                _logger.Info("Api retrieved rows by sku ");
                if (response.IsSuccessStatusCode)
                {
                    _logger.Info("Api response success");
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
                    response = await client.PutAsync("api/products/" + id, stringContent);
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