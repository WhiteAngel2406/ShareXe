using System.ComponentModel.DataAnnotations;

namespace ShareXe.src.Modules.Trips.Dtos
{
    public class CreateTripDto
    {
        [Required]
        public Guid VehicleId { get; set; }

        [Required]
        public Guid StartHubId { get; set; }

        [Required]
        public Guid EndHubId { get; set; }

        [Required]
        public DateTimeOffset DepartureTime { get; set; }

        [Required]
        public DateTimeOffset EstimatedArrivalTime { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Price per seat must be a positive number.")]
        public decimal PricePerSeat { get; set; }

        [MaxLength(500)]
        public string? Note { get; set; }
    }
}
