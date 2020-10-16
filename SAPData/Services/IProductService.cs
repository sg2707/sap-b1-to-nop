using SAPData.Models;
using System.Collections.Generic;

namespace SAPData.Services
{
    public interface IProductService
    {
        //method to read product
        List<NOPCommerceProduct> getProductList();
    }
}