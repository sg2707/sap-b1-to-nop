using NopAPIConnect.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NopAPIConnect
{
    public interface INopAPIConnect
    {
        //void SaveProducts();
        Task SaveProductsAsync(List<NOPCommerceApiProduct> products);
        Task SaveCategoriesAsync(List<NopCommerceApiCategory> categories);
        Task SaveManufacturesAsync(List<NOPCommerceApiManufactures> manufacturers);
        Task SaveSpecificationAttributeAsync(List<NOPCommerceApiSpecificationAttribute> Specattributes);
        Task<List<NopCommerceApiOrder>> GetOrdersAsync();
    }
}