using AutoMapper;

using ShareXe.Modules.Auth.Entities;
using ShareXe.Modules.Users.Dtos;

namespace ShareXe.Modules.Auth.Mappers
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            CreateMap<Account, UserProfileDto>()
              .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User != null ? src.User.FullName : null))
              .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.User != null ? src.User.Avatar : null));
        }
    }
}
