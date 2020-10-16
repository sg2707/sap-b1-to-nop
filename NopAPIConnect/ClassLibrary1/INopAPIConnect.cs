using NopAPIConnect.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NopAPIConnect
{
    public interface INopAPIConnect
    {
        //void SaveProducts();
        Task SaveProductsAsync(List<NOPCommerceApiProduct> products);
    }
}