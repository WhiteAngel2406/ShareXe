using ShareXe.Modules.Minio.Dtos;

namespace ShareXe.src.Modules.Vehicles.Dtos
{
    public class VehicleDto
    {
        public Guid DriverId { get; set; }
        public Guid TypeId { get; set; }
        public string PlateNumber { get; set; } = null!;
        public string Model { get; set; } = null!;
        public string Color { get; set; } = null!;
        public bool IsActive { get; set; }

        // Trả về dạng Object chứa URL (đã được MinioService gen ra)
        public MinioFileResponse? Image { get; set; }
    }
}
