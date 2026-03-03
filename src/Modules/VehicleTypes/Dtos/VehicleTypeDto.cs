using ShareXe.Base.Dtos;

namespace ShareXe.src.Modules.VehicleTypes.Dtos
{
    public class VehicleTypeDto : EntityDto
    {
        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public int TotalSeat { get; set; }
    }
}
