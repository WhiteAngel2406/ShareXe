using ShareXe.Base.Dtos;

namespace ShareXe.src.Modules.SeatTemplates.Dtos
{
    public class SeatTemplateDto : EntityDto
    {
        public Guid VehicleTypeId { get; set; }
        public string SeatCode { get; set; } = null!;
        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public bool IsDriverSeat { get; set; }
    }
}
