using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopAPIConnect.Models
{
   public class NopCommerceApiCategory
    {
        public int id { get; set; }
        public string name { get; set; }

        //public string description { get; set; }

        //public int? category_template_id { get; set; }

        public string meta_keywords { get; set; }

        public string parent_meta_keywords { get; set; }


        //public string meta_description { get; set; }

        //public string meta_title { get; set; }

        public int? parent_category_id { get; set; }

        //public int? page_size { get; set; }

        //public string page_size_options { get; set; }


        //public string price_ranges { get; set; }


        //public bool? show_on_home_page { get; set; }

        //public bool? include_in_top_menu { get; set; }

        //public bool? has_discounts_applied { get; set; }


        //public bool? published { get; set; }

        //public bool? deleted { get; set; }

        //public int? display_order { get; set; }

        //public DateTime? created_on_utc { get; set; }
        //public DateTime? updated_on_utc { get; set; }


        //public List<int> role_ids { get; set; }

        //public List<int> discount_ids { get; set; }

        //public List<int> store_ids { get; set; }

        //public string se_name { get; set; }
    }

    public class RootObjectCtg
    {
        public List<NopCommerceApiCategory> categories { get; set; }
    }
}

