using AutoMapper;

using ShareXe.Modules.Users.Dtos;
using ShareXe.Modules.Users.Entities;

namespace ShareXe.Modules.Users.Mappers
{
    public class UsersProfile : Profile
    {
        public UsersProfile()
        {
            CreateMap<User, UserProfileDto>()
              .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Account.Email))
              .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Account.Role));
        }
    }
}
