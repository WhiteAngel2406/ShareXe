namespace ShareXe.src.Modules.SeatTemplates.Entities
{
    public class SeatTemplate
    {
        public Guid VehicleTypeId { get; set; }
        public String SeatCode { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public bool isDriverSeated { get; set; }
    }
}
