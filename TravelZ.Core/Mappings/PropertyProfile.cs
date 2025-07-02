using AutoMapper;
using TravelZ.Core.DTOs;
using TravelZ.Core.Models;

namespace TravelZ.Core.Mappings
{
    public class PropertyProfile : Profile
    {
        public PropertyProfile()
        {
            CreateMap<Property, PropertyDto>()
                .ForMember(dest => dest.Owner, opt => opt.Ignore());
            CreateMap<PropertyDto, Property>()
                .ForMember(dest => dest.Owner, opt => opt.Ignore());
        }
    }
}
