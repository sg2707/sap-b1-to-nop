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
    public class VehicleService : IVehicleService
    {
        public List<NOPCommerceVehicle> GetVehicleList(DateTime LastVehicleSync)
        {
            DBContext dc = new DBContext();
            if (!dc.Database.Exists())
                dc.Database.Connection.Open();
            dc.Database.CommandTimeout = 120;
            var vehicles = dc.Database.SqlQuery<NOPCommerceVehicle>(
            "exec SI_NopCommerceVehicle @LastVehicleSync", new SqlParameter("LastVehicleSync", LastVehicleSync)).ToList();
            return vehicles;
        }
    }
}
