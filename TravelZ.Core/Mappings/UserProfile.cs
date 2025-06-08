using AutoMapper;
using TravelZ.Core.DTOs;
using TravelZ.Core.Models;

namespace TravelZ.Core.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Roles, opt => opt.Ignore()); // Roles are handled separately
        }
    }
}
