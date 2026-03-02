using AutoMapper;

using ShareXe.Modules.Vehicles.Entities;
using ShareXe.src.Modules.Vehicles.Dtos;

namespace ShareXe.src.Modules.Vehicles.Mappers
{
    public class VehiclesProfile : Profile
    {
        public VehiclesProfile()
        {
            // Map từ Entity (Vehicle) sang DTO trả về (VehicleDto)
            CreateMap<Vehicle, VehicleDto>()
                // Bỏ qua trường Image vì xử lý URL bằng MinioService trong VehiclesService
                .ForMember(dest => dest.Image, opt => opt.Ignore());

            // Map từ DTO tạo mới (CreateVehicleDto) sang Entity (Vehicle)
            CreateMap<CreateVehicleDto, Vehicle>()
                // Bỏ qua trường Image vì IFormFile không map trực tiếp sang string được
                // Tên file sẽ được gán sau khi upload lên Minio thành công
                .ForMember(dest => dest.Image, opt => opt.Ignore());
        }
    }
}
