using SAPData.Models;
using SAPData.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace SAPData
{
    public class ProductService :  IProductService
    {
        public List<NOPCommerceProduct> GetProductList(DateTime LastProdSyncDate)
        {
                DBContext dc = new DBContext();
                if (!dc.Database.Exists())
                dc.Database.Connection.Open();
                var products = dc.Database.SqlQuery<NOPCommerceProduct>(
                "exec SI_NopCommerceProduct @LastProdSyncDate",new SqlParameter("LastProdSyncDate", LastProdSyncDate)).ToList();
                return products;
        }
    }
}
