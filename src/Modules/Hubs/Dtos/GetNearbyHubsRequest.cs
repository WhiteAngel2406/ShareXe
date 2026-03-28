using System.ComponentModel.DataAnnotations;

namespace ShareXe.Modules.Hubs.Dtos
{
    public class GetNearbyHubsRequest
    {
        [Required]
        [Range(-90, 90)]
        public double Latitude { get; set; }

        [Required]
        [Range(-180, 180)]
        public double Longitude { get; set; }
    }
}