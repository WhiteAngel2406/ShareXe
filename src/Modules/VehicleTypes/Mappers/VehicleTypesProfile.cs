using AutoMapper;

using ShareXe.Modules.VehicleTypes.Entities;
using ShareXe.src.Modules.VehicleTypes.Dtos;

namespace ShareXe.src.Modules.VehicleTypes.Mappers
{
    public class VehicleTypesProfile : Profile
    {
        public VehicleTypesProfile()
        {
            // Ánh xạ từ Entity (VehicleType) sang DTO trả về (VehicleTypeDto)
            CreateMap<VehicleType, VehicleTypeDto>();

            // Ánh xạ từ DTO đầu vào (CreateVehicleTypeDto) sang Entity (VehicleType)
            CreateMap<CreateVehicleTypeDto, VehicleType>();
        }
    }
}
