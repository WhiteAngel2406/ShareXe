using ShareXe.Base.Dtos;

namespace ShareXe.Modules.Hubs.Dtos
{
    public class HubDto : EntityDto
    {
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Description { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}