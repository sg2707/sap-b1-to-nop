using NopAPIConnect.Models;
using SAPData.Models;
using AutoMapper;

namespace ProductModule
{
    public class MapperConfiguration : Profile
    {
        public MapperConfiguration()
        {
            CreateMap<NOPCommerceProduct, NOPCommerceApiProduct>().ForMember(model => model.manufacturer_ids, opt => opt.MapFrom(src => src.manufacturer_ids));
            //CreateMap<NOPCommerceApiProduct, NOPCommerceProduct>();
        }

    }
}
