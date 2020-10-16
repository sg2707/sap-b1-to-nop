namespace SAPData.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CompanyManagement")]
    public partial class CompanyManagement
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string IPAddress { get; set; }

        [StringLength(50)]
        public string SAPDatabase { get; set; }

        [StringLength(50)]
        public string UserID { get; set; }

        [StringLength(50)]
        public string Password { get; set; }

        [StringLength(50)]
        public string CompanyName { get; set; }

        [StringLength(50)]
        public string Country { get; set; }

        [StringLength(50)]
        public string SAPVersion { get; set; }

        //[StringLength(50)]
        //public string StaffInCharged { get; set; }

        //[StringLength(50)]
        //public string ContactNumber { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        //[StringLength(500)]
        //public string ErrorMsg { get; set; }

        [StringLength(50)]
        public string SAPUserName { get; set; }

        [StringLength(50)]
        public string SAPPassword { get; set; }

        [StringLength(2)]
        public string CompanyCode { get; set; }

        public bool? IsActive { get; set; }

        //[StringLength(20)]
        //public string LicenseServer { get; set; }

        //[StringLength(5)]
        //public string DBServerType { get; set; }
    }
}
