using NopAPIConnect.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities;

namespace NopAPIConnect
{
    public interface INopAPIConnect
    {
        //void SaveProducts();
        Task SaveProductsAsync(List<NOPCommerceApiProduct> products, ProgressBinder progress);
        Task SaveCategoriesAsync(List<NopCommerceApiCategory> categories);
        Task SaveManufacturesAsync(List<NOPCommerceApiManufactures> manufacturers);
        Task SaveSpecificationAttributeAsync(List<NOPCommerceApiSpecificationAttribute> Specattributes);
        Task<List<NopCommerceApiOrder>> GetOrdersAsync();
        void SaveVehicle(List<NOPCommerceApiVehicle> vehicles);
    }
}