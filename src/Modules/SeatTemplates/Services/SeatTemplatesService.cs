using AutoMapper;

using ShareXe.Base.Attributes;
using ShareXe.Base.Enums;
using ShareXe.Base.Exceptions;
using ShareXe.Base.Repositories;
using ShareXe.Modules.SeatTemplates.Entities;
using ShareXe.src.Modules.SeatTemplates.Dtos;
using ShareXe.src.Modules.SeatTemplates.Repositories;

namespace ShareXe.src.Modules.SeatTemplates.Services
{
    [Injectable]
    public class SeatTemplatesService(
         SeatTemplatesRepository seatTemplatesRepository,
         IMapper mapper)
    {
        public async Task<SeatTemplateDto> CreateSeatTemplateAsync(CreateSeatTemplateDto createDto)
        {
            // 1. Kiểm tra xem mã ghế đã tồn tại trong loại xe này chưa (VD: Đã có ghế A1 cho xe 4 chỗ chưa)
            var existingCode = await seatTemplatesRepository.GetOneAsync(st =>
                st.VehicleTypeId == createDto.VehicleTypeId &&
                st.SeatCode.ToLower() == createDto.SeatCode.ToLower());

            if (existingCode != null)
            {
                throw new AppException(ErrorCode.InvalidRequest, $"Mã ghế '{createDto.SeatCode}' đã tồn tại cho loại xe này.");
            }

            // 2. Kiểm tra xem tọa độ (X, Y) đã bị chiếm dụng chưa (Tránh việc vẽ đè 2 ghế lên nhau)
            var existingPosition = await seatTemplatesRepository.GetOneAsync(st =>
                st.VehicleTypeId == createDto.VehicleTypeId &&
                st.PositionX == createDto.PositionX &&
                st.PositionY == createDto.PositionY);

            if (existingPosition != null)
            {
                throw new AppException(ErrorCode.InvalidRequest, $"Vị trí tọa độ ({createDto.PositionX}, {createDto.PositionY}) đã có ghế khác chiếm dụng.");
            }

            // 3. Map sang Entity và lưu DB
            var seatTemplate = mapper.Map<SeatTemplate>(createDto);
            await seatTemplatesRepository.AddAsync(seatTemplate);

            // 4. Trả về kết quả
            return mapper.Map<SeatTemplateDto>(seatTemplate);
        }

        public async Task<IEnumerable<SeatTemplateDto>> GetTemplatesByVehicleTypeIdAsync(Guid vehicleTypeId)
        {
            // Lấy toàn bộ danh sách ghế của một loại xe cụ thể
            var queryOptions = new QueryOptions<SeatTemplate>
            {
                Filter = st => st.VehicleTypeId == vehicleTypeId
                // Tùy chọn: Sắp xếp theo X rồi theo Y để lúc trả về danh sách được mượt mà hơn
                // OrderBy = q => q.OrderBy(st => st.PositionY).ThenBy(st => st.PositionX)
            };

            var templates = await seatTemplatesRepository.GetAllAsync(queryOptions);

            return mapper.Map<IEnumerable<SeatTemplateDto>>(templates);
        }
    }
}
