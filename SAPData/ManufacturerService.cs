using SAPData.Models;
using SAPData.Services;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPData
{
   public class ManufacturerService : IManufacturerService
    {
        public List<NOPCommerceManufactures> GetManufacturerList(DateTime LastManfSyncDate)
        {
            DBContext dc = new DBContext();
            if (!dc.Database.Exists())
                dc.Database.Connection.Open();
            dc.Database.CommandTimeout = 120;
            var manufacturer = dc.Database.SqlQuery<NOPCommerceManufactures>(
            "exec SI_NopCommerceManufacturer @LastManfSyncDate", new SqlParameter("LastManfSyncDate", LastManfSyncDate)).ToList();
            return manufacturer;
        }
    }
}
