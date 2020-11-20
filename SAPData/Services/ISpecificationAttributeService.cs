using SAPData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPData.Services
{

public interface ISpecificationAttributeService
{
        List<NOPCommerceSpecificationAttribute> GetSpecificationAttributeList(DateTime LastSpecSync);
}
}
