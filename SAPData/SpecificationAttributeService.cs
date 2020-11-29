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
    public class  SpecificationAttributeService : ISpecificationAttributeService
    {
        public List<NOPCommerceSpecificationAttribute> GetSpecificationAttributeList(DateTime LastSpecSync)
        {
            DBContext dc = new DBContext();
            if (!dc.Database.Exists())
                dc.Database.Connection.Open();
            dc.Database.CommandTimeout = 120;
            var specattribute =dc.Database.SqlQuery<NOPCommerceSpecificationAttribute>(
           "exec SI_NopCommerceProductSpecification @LastSpecSync", new SqlParameter("LastSpecSync", LastSpecSync)).ToList();
            return specattribute;
        }

        public List<NOPCommerceSpecificationAttribute> GetSpecificationAttributeListBySku(string ProductSku)
        {
            DBContext dc = new DBContext();
            if (!dc.Database.Exists())
                dc.Database.Connection.Open();
            dc.Database.CommandTimeout = 120;
            var specattribute = dc.Database.SqlQuery<NOPCommerceSpecificationAttribute>(
               "exec SI_NopCommerceProductSpecMapping @ProductSku", new SqlParameter("ProductSku", ProductSku)).ToList();
            return specattribute;
        }


        public List<SpecificationAttributeOptions> GetSpecificationOptionsList(string AttributeId)
        {
            DBContext dc = new DBContext();
            if (!dc.Database.Exists())
                dc.Database.Connection.Open();
            dc.Database.CommandTimeout = 120;
            var specoption = dc.Database.SqlQuery<SpecificationAttributeOptions>(
           "exec SI_NopCommerceSpecificationOptions @AttributeId", new SqlParameter("AttributeId", AttributeId)).ToList();
            return specoption;
        }


    }
}
