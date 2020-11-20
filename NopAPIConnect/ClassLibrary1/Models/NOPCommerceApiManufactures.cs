using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopAPIConnect.Models
{
    public class NOPCommerceApiManufactures
    {
        public int id { get; set; }
        public string name { get; set; }


        //public List<localized_names> LocalizedNames { get; set; }

        //public string description { get; set; }

        //public int manufacturer_template_id { get; set; }


        //public string meta_keywords { get; set; }
        //public string meta_description { get; set; }

        //public string meta_title { get; set; }

        //public int picture_id { get; set; }

        //public int page_size { get; set; }


        //public bool allow_customers_to_select_page_size { get; set; }


        //public string page_size_options { get; set; }


        //public string price_ranges { get; set; }

       // public bool subject_to_acl { get; set; }


        //public bool limited_to_stores { get; set; }

        //public bool published { get; set; }

        //public bool deleted { get; set; }

        //public int display_order { get; set; }


        //public DateTime created_on_utc { get; set; }


        //public DateTime updated_on_utc { get; set; }


        //public List<int> role_ids { get; set; }


        //public List<int> discount_ids { get; set; }


        //public List<int> store_ids { get; set; }


        //  public ImageDto image { get; set; }

        //public string se_name { get; set; }
    }

    public class RootObject
    {
        public List<NOPCommerceApiManufactures> manufacturers { get; set; }
    }
}
