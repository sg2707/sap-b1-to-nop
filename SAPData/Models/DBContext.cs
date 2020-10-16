namespace SAPData.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class DBContext : DbContext
    {
        public DBContext()
            : base("name=SAP")
        {
        }

        public virtual DbSet<CompanyManagement> CompanyManagements { get; set; }

        //public virtual DbSet<NOPCommerceProduct> NOPCommerceProduct { get; set; }
        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{

        //}
    }
}
