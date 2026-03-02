
using AutoMapper;

using ShareXe.Base.Attributes;
using ShareXe.Base.Enums;
using ShareXe.Base.Exceptions;
using ShareXe.Modules.Auth;
using ShareXe.Modules.Minio.Dtos;
using ShareXe.Modules.Minio.Services;
using ShareXe.Modules.Vehicles.Entities;
using ShareXe.src.Modules.DriverProfiles.Repositories;
using ShareXe.src.Modules.Vehicles.Dtos;
using ShareXe.src.Modules.Vehicles.Repositories;

namespace ShareXe.src.Modules.Vehicles.Services

{
    [Injectable]
    public class VehiclesService(
         VehiclesRepository vehiclesRepository,
         DriverProfilesRepository driverProfilesRepository, // Gọi sang module Driver để check
         MinioService minioService,
         UserContext userContext,
         IMapper mapper)
    {
        // Hàm phụ trợ: Lấy ID của Driver Profile hiện tại
        private async Task<Guid> GetCurrentDriverIdAsync()
        {
            var firebaseUid = userContext.FirebaseUid;

            if (string.IsNullOrEmpty(firebaseUid))
                throw new AppException(ErrorCode.Unauthorized, "No authenticated user.");

            // Tìm hồ sơ tài xế dựa trên FirebaseUid của user
            var driverProfile = await driverProfilesRepository.GetOneAsync(dp => dp.User.Account.FirebaseUid == firebaseUid);

            if (driverProfile == null)
                throw new AppException(ErrorCode.InvalidRequest, "You must register as a driver before adding vehicles.");

            return driverProfile.Id;
        }

        public async Task<Vehicle> CreateVehicleAsync(CreateVehicleDto createVehicleDto)
        {
            var driverId = await GetCurrentDriverIdAsync();

            // Kiểm tra xem biển số xe đã tồn tại trong hệ thống chưa
            var existingVehicle = await vehiclesRepository.GetOneAsync(v => v.PlateNumber == createVehicleDto.PlateNumber);
            if (existingVehicle != null)
            {
                throw new AppException(ErrorCode.ValidationError, "This plate number is already registered.");
            }

            // Upload ảnh xe lên Minio, lưu vào thư mục chứa ID của tài xế cho dễ quản lý
            var folder = $"vehicles/{driverId}";
            var imageResponse = await minioService.UploadFileAsync(createVehicleDto.Image, folder);

            var vehicle = new Vehicle
            {
                DriverId = driverId,
                TypeId = createVehicleDto.TypeId,
                PlateNumber = createVehicleDto.PlateNumber,
                Model = createVehicleDto.Model,
                Color = createVehicleDto.Color,
                Image = imageResponse.FileName, // Lưu tên file từ Minio trả về
                IsActive = true
            };

            await vehiclesRepository.AddAsync(vehicle);

            return vehicle;
        }

        public async Task<IEnumerable<Vehicle>> GetMyVehiclesAsync()
        {
            var driverId = await GetCurrentDriverIdAsync();

            // Trả về danh sách xe của tài xế này
            return await vehiclesRepository.GetAsync(v => v.DriverId == driverId);
        }

        public async Task<VehicleDto> MapToVehicleDtoAsync(Vehicle vehicle)
        {
            var vehicleDto = mapper.Map<VehicleDto>(vehicle);

            if (!string.IsNullOrEmpty(vehicle.Image))
            {
                vehicleDto.Image = new MinioFileResponse
                {
                    FileName = vehicle.Image,
                    Url = await minioService.GeneratePresignedUrlAsync(vehicle.Image)
                };
            }

            return vehicleDto;
        }
    }
}
