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
    public class CategoryService : ICategoryService
    {
        public List<NopCommerceCategory> GetCategoryList()
        {
            DBContext dc = new DBContext();
            if (!dc.Database.Exists())
                dc.Database.Connection.Open();
            dc.Database.CommandTimeout = 120;
            var category = dc.Database.SqlQuery<NopCommerceCategory>(
            "exec SI_NopCommerceCategory").ToList();
            return category;
        }
    }
}
