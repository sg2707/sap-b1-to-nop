using SAPData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopAPIConnect.Models
{
    public class NOPCommerceApiSpecificationAttribute
    {
        public string name { get; set; }
        public string attribute_id { get; set; }

        public int control_type { get; set; }

        public List<SpecificationAttributeOptions> specification_attribute_options { get; set; }

    }
    public class RootObjectSpec
    {
        public List<NOPCommerceApiSpecificationAttribute> specification_attributes { get; set; }
    }
}
