using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPData.Models
{
  public  class SpecificationAttributeOptions
    {

       public int id { get; set; }
        public string name { get; set; }

        public int specification_attribute_id { get; set; }
        public string color_squares_rgb { get; set; }

        public int display_order { get; set; }
    }

  
}
