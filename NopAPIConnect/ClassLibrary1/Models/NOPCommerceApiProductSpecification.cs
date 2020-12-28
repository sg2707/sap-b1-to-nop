using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopAPIConnect.Models
{
   public class NOPCommerceApiProductSpecification
    {
        public int id { get; set; }

        public int product_id { get; set; }

        public int attribute_type_id { get; set; }

        public int specification_attribute_option_id { get; set; }

        public string custom_value { get; set; }

        public bool allow_filtering { get; set; }

        public bool show_on_product_page { get; set; }

        public int display_order { get; set; }

        public string attribute_type { get; set; }
       

    }

    public class RootObjectProductSpecMap
    {
        public List<NOPCommerceApiProductSpecification> product_specification_attributes { get; set; }
    }
}
