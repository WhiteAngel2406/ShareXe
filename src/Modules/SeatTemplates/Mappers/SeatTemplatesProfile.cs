using AutoMapper;

using ShareXe.Modules.SeatTemplates.Entities;
using ShareXe.Modules.SeatTemplates.Dtos;

namespace ShareXe.Modules.SeatTemplates.Mappers
{
    public class SeatTemplatesProfile : Profile
    {
        public SeatTemplatesProfile()
        {
            // Ánh xạ từ Request tạo mới sang Entity
            CreateMap<CreateSeatTemplateDto, SeatTemplate>();

            // Ánh xạ từ Entity sang Response DTO để trả về
            CreateMap<SeatTemplate, SeatTemplateDto>();
        }
    }
}
