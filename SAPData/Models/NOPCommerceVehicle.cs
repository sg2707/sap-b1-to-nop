using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPData.Models
{
    public class NOPCommerceVehicle
    {
        public string Make { get; set; }

        public string ModelNo { get; set; }
        public string ModelName { get; set; }

        public string VehicleChassisGrp { get; set; }


        public string Chassis { get; set; }


        public string VehicleEngineGrp { get; set; }

        public string Engine { get; set; }
   
        public string CCPrefix { get; set; }

        public string CC { get; set; }

        public string CCSufix { get; set; }

        public string HandDrive { get; set; }

        public string TransmissionType { get; set; }

        public string TransmissionCode { get; set; }

        public string FuelType { get; set; }

        public string CountryOfManufacture { get; set; }

        public DateTime ManufactureStart { get; set; }

        public DateTime ManufactureEnd { get; set; }

        public string LastModifiedBy { get; set; }

        public DateTime LastModifiedDate { get; set; }

        public int SAPVehicleId { get; set; }

    }
}
