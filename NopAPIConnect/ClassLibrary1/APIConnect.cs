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


namespace NopAPIConnect
{
    public class NopAPIConnect : INopAPIConnect
    {
        public string AccessToken { get; set; }
        public static HttpClient client { get; set; }
        public HttpResponseMessage response { get; set; }
        public NopAPIConnect()
        {
            client = new HttpClient();
            response = new HttpResponseMessage();
            AccessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYmYiOiIxNjAyNTg0NTc0IiwiZXhwIjoiMTkxNzk0NDU3NCIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6ImFkbWluQHlvdXJTdG9yZS5jb20iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6ImM4YjdkMTg5LTBhN2QtNDcwOS04MTBhLWNiYTQyMmUyNzZiOCIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJhZG1pbkB5b3VyU3RvcmUuY29tIn0.qvMFPvvf1Puvs79JloBfNtSXs7JUlZ8rHJoXxp0kUwo";
            client.BaseAddress = new Uri("http://localhost:70/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            //get values from config to initialize the client
        }
        //here implement service
        public async Task SaveProductsAsync(List<NOPCommerceApiProduct> products)
        {
            string sku = null;
            int id = 0;
            foreach (var product in products)
            {
                response = await client.GetAsync("api/products?sku=" + product.sku);
                if (response.IsSuccessStatusCode)
                {
                    var prodresponse = response.Content.ReadAsStringAsync().Result;
                    dynamic newproducts = JsonConvert.DeserializeObject(prodresponse);
                    sku = newproducts.products[0].sku;
                    id = newproducts.products[0].id;
                }
                var output = "{  \"product\": " + JsonConvert.SerializeObject(product) + "}";
                var stringContent = new StringContent(output);

                if (sku == null)
                {
                    response = await client.PostAsync("api/products", stringContent);
                }
                else
                {
                    response = await client.PutAsync("api/products/" + id, stringContent);
                }
            }
        }

    }
}