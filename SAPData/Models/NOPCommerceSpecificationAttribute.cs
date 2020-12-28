using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPData.Models
{
   public class NOPCommerceSpecificationAttribute
    {
       
        public string name { get; set; }

        public string attribute_id { get; set; }

        public int control_type { get; set; }

        public string option_name { get; set; }
        public string custom_text { get; set; }

        public List<SpecificationAttributeOptions> specification_attribute_options { get; set; }

    }
}
