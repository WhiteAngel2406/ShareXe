using AutoMapper;

using ShareXe.Base.Attributes;
using ShareXe.Base.Enums;
using ShareXe.Base.Exceptions;
using ShareXe.Base.Repositories;
using ShareXe.Modules.VehicleTypes.Entities;
using ShareXe.Modules.VehicleTypes.Dtos;
using ShareXe.Modules.VehicleTypes.Repositories;

namespace ShareXe.Modules.VehicleTypes.Services
{
    [Injectable]
    public class VehicleTypesService(
         VehicleTypesRepository vehicleTypesRepository,
         IMapper mapper)
    {
        public async Task<IEnumerable<VehicleTypeDto>> GetAllVehicleTypesAsync()
        {
            // Lấy toàn bộ danh sách loại xe từ Database
            // Vì danh sách loại xe thường không nhiều, ta có thể lấy hết mà không cần phân trang
            var vehicleTypes = await vehicleTypesRepository.GetAllAsync(new QueryOptions<VehicleType>());

            // Map từ List<VehicleType> sang List<VehicleTypeDto>
            return mapper.Map<IEnumerable<VehicleTypeDto>>(vehicleTypes);
        }

        public async Task<VehicleTypeDto> CreateVehicleTypeAsync(CreateVehicleTypeDto createDto)
        {
            // 1. Kiểm tra xem tên loại xe đã tồn tại chưa (VD: "Xe 4 chỗ" hoặc "Xe 7 chỗ")
            var existingType = await vehicleTypesRepository.GetOneAsync(vt => vt.Name.ToLower() == createDto.Name.ToLower());
            if (existingType != null)
            {
                throw new AppException(ErrorCode.InvalidRequest, $"Loại xe với tên '{createDto.Name}' đã tồn tại trong hệ thống.");
            }

            // 2. Map từ DTO tạo mới sang Entity
            var vehicleType = mapper.Map<VehicleType>(createDto);

            // 3. Lưu vào Database
            await vehicleTypesRepository.AddAsync(vehicleType);

            // 4. Map ngược lại ra DTO để trả về cho Client
            return mapper.Map<VehicleTypeDto>(vehicleType);
        }
    }
}
