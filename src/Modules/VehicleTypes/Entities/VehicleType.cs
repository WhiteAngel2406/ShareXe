using ShareXe.src.Modules.SeatTemplates.Entities;

namespace ShareXe.src.Modules.VehicleTypes.Entities
{
    public class VehicleType
    {
        public String Name { get; set; }
        public String Description { get; set; }
        public int TotalSeat { get; set; }
        public ICollection<SeatTemplate> VehicleTypes { get; set; }
    }
}
