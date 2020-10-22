using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopAPIConnect.Models
{
    public class NOPCommerceApiProduct
    {

        //private int? _productTypeId;
        //public bool? visible_individually { get; set; }

        public string name { get; set; }

        // public List<localized_names> LocalizedNames { get; set; }

        public string short_description { get; set; }

        public string full_description { get; set; }


        //public bool? show_on_home_page { get; set; }

        //public string meta_keywords { get; set; }

        //public string meta_description { get; set; }

        //public string meta_title { get; set; }

        //public bool? allow_customer_reviews { get; set; }

        //public int? approved_rating_sum { get; set; }

        //public int? not_approved_rating_sum { get; set; }

        //public int? approved_total_reviews { get; set; }

        //public int? not_approved_total_reviews { get; set; }

        public string sku { get; set; }

        //public string manufacturer_part_number { get; set; }

        //public string gtin { get; set; }

        //public bool? is_gift_card { get; set; }

        //public bool? require_other_products { get; set; }

        //public bool? automatically_add_required_products { get; set; }

        //public bool? is_download { get; set; }

        //public bool? unlimited_downloads { get; set; }
        //public int? max_number_of_downloads { get; set; }

        //public int? download_expiration_days { get; set; }

        //public bool? has_sample_download { get; set; }

        //public bool? has_user_agreement { get; set; }

        //public bool? is_recurring { get; set; }

        //public int? recurring_cycle_length { get; set; }

        //public int? recurring_total_cycles { get; set; }

        //public bool? is_rental { get; set; }

        //public int? rental_price_length { get; set; }
        //public bool? is_ship_enabled { get; set; }

        //public bool? is_free_shipping { get; set; }

        //public bool? ship_separately { get; set; }

        //public decimal? additional_shipping_charge { get; set; }

        //public bool? is_tax_exempt { get; set; }

        //public bool? is_telecommunications_or_broadcasting_or_electronic_services { get; set; }

        //public bool? use_multiple_warehouses { get; set; }

        //public int? manage_inventory_method_id { get; set; }

         public int? stock_quantity { get; set; }
        //public bool? display_stock_availability { get; set; }

        //public bool? display_stock_quantity { get; set; }


        //public int? min_stock_quantity { get; set; }

        //public int? notify_admin_for_quantity_below { get; set; }

        //public bool? allow_back_in_stock_subscriptions { get; set; }


        //public int? order_minimum_quantity { get; set; }

        //public int? order_maximum_quantity { get; set; }


        //public string allowed_quantities { get; set; }

        //public bool? allow_adding_only_existing_attribute_combinations { get; set; }


        //public bool? disable_buy_button { get; set; }


        //public bool? disable_wishlist_button { get; set; }


        //public bool? available_for_pre_order { get; set; }


        //public DateTime? pre_order_availability_start_date_time_utc { get; set; }

        //public bool? call_for_price { get; set; }

        public decimal? price { get; set; }


        //public decimal? old_price { get; set; }

        //public decimal? product_cost { get; set; }


        //public decimal? special_price { get; set; }


        //public DateTime? special_price_start_date_time_utc { get; set; }


        //public DateTime? special_price_end_date_time_utc { get; set; }


        //public bool? customer_enters_price { get; set; }

        //public decimal? minimum_customer_entered_price { get; set; }

        //public decimal? maximum_customer_entered_price { get; set; }

        //public bool? baseprice_enabled { get; set; }

        //public decimal? baseprice_amount { get; set; }

        //public decimal? baseprice_base_amount { get; set; }


        //public bool? has_tier_prices { get; set; }


        //public bool? has_discounts_applied { get; set; }

        //public decimal? weight { get; set; }

        //public decimal? length { get; set; }


        //public decimal? width { get; set; }


        //public decimal? height { get; set; }

        //public DateTime? available_start_date_time_utc { get; set; }

        //public DateTime? available_end_date_time_utc { get; set; }

        //public int? display_order { get; set; }

        //public bool? published { get; set; }

        //public bool? deleted { get; set; }

        //public DateTime? created_on_utc { get; set; }


        //public DateTime? updated_on_utc { get; set; }


        ////public string product_type
        ////{
        ////    get
        ////    {
        ////        var productTypeId = _productTypeId;
        ////        if (productTypeId != null)
        ////        {
        ////            return ((ProductType)productTypeId).ToString();
        ////        }

        ////        return null;
        ////    }
        ////    set
        ////    {
        ////        ProductType productTypeId;
        ////        if (Enum.TryParse(value, out productTypeId))
        ////        {
        ////            _productTypeId = (int)productTypeId;
        ////        }
        ////        else
        ////        {
        ////            _productTypeId = null;
        ////        }
        ////    }
        ////}


        //public int? parent_grouped_product_id { get; set; }


        //public List<int> role_ids { get; set; }


        //public List<int> discount_ids { get; set; }


        //public List<int> store_ids { get; set; }


        public List<int> manufacturer_ids { get; set; }

        // public List<ImageMapping> images { get; set; }


        //public List<ProductAttributeMapping> attributes { get; set; }


        // public List<ProductAttributeCombination> product_attribute_combinations { get; set; }



        // public List<ProductSpecificationAttribute> product_specification_attributes { get; set; }


        //public List<int> associated_product_ids { get; set; }

        //public List<string> tags { get; set; }

        //public int? vendor_id { get; set; }

        //public string se_name { get; set; }

        public List<int> category_ids { get; set; }
    }
}


