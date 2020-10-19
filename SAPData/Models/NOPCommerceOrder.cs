using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPData
{
    public class NOPCommerceOrder
    {
        public int? StoreId { get; set; }

      
        public bool? PickUpInStore { get; set; }

      
        public string PaymentMethodSystemName { get; set; }

        public string CustomerCurrencyCode { get; set; }

       
        public decimal? CurrencyRate { get; set; }

    
        public int? CustomerTaxDisplayTypeId { get; set; }

      
        public string VatNumber { get; set; }

        public decimal? OrderSubtotalInclTax { get; set; }

      
        public decimal? OrderSubtotalExclTax { get; set; }

     
        public decimal? OrderSubTotalDiscountInclTax { get; set; }

        public decimal? OrderSubTotalDiscountExclTax { get; set; }

       
        public decimal? OrderShippingInclTax { get; set; }

       
        public decimal? OrderShippingExclTax { get; set; }

      
        public decimal? PaymentMethodAdditionalFeeInclTax { get; set; }

     
        public decimal? PaymentMethodAdditionalFeeExclTax { get; set; }

       
        public string TaxRates { get; set; }

       
        public decimal? OrderTax { get; set; }

      
        public decimal? OrderDiscount { get; set; }

       
        public decimal? OrderTotal { get; set; }

      
        public decimal? RefundedAmount { get; set; }

       
        public bool? RewardPointsWereAdded { get; set; }

       
        public string CheckoutAttributeDescription { get; set; }

       
        public int? CustomerLanguageId { get; set; }

       
        public int? AffiliateId { get; set; }

       
        public string CustomerIp { get; set; }

       
        public string AuthorizationTransactionId { get; set; }

        public string AuthorizationTransactionCode { get; set; }

     
       
        public string AuthorizationTransactionResult { get; set; }

     
        public string CaptureTransactionId { get; set; }

        
        public string CaptureTransactionResult { get; set; }

        
        public string SubscriptionTransactionId { get; set; }

      
        public DateTime? PaidDateUtc { get; set; }

        public string ShippingMethod { get; set; }

        
        public string ShippingRateComputationMethodSystemName { get; set; }

     
        public string CustomValuesXml { get; set; }

      
        public bool? Deleted { get; set; }

       
        public DateTime? CreatedOnUtc { get; set; }

      
       /// public OrderCustomerDto Customer { get; set; }

        //public int? CustomerId { get; set; }

        
       // public AddressDto BillingAddress { get; set; }

       
        //public AddressDto ShippingAddress { get; set; }

       
        //public ICollection<OrderItemDto> OrderItems
        //{
        //    get
        //    {
        //        if (_orderItems == null)
        //        {
        //            _orderItems = new List<OrderItemDto>();
        //        }

        //        return _orderItems;
        //    }
        //    set => _orderItems = value;
        //}

       
        public string OrderStatus { get; set; }

        public string PaymentStatus { get; set; }

       
        public string ShippingStatus { get; set; }

     
        public string CustomerTaxDisplayType { get; set; }
    }
}
