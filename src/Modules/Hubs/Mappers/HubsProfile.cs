using AutoMapper;

using ShareXe.Modules.Hubs.Dtos;
using ShareXe.Modules.Hubs.Entities;

namespace ShareXe.Modules.Hubs.Mappers
{
    public class HubsProfile : Profile
    {
        public HubsProfile()
        {
            CreateMap<Hub, HubDto>();
        }
    }
}