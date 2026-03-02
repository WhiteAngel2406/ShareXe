using ShareXe.Base.Attributes;
using ShareXe.Base.Entities;
using ShareXe.Modules.Users.Entities;
using ShareXe.Modules.Vehicles.Entities;

using System.ComponentModel.DataAnnotations;

namespace ShareXe.Modules.DriverProfiles.Entities
{
    [Entity("driver_profiles")]
    public class DriverProfile : BaseEntity
    {
        [Required]
        [Unique]
        public Guid UserId { get; set; }

        public User User { get; set; } = null!;

        [Required]
        [MaxLength(20)]
        public string NationalIdCard { get; set; } = null!;

        public double RatingScore { get; set; }

        public int TotalTrips { get; set; }

        [Required]
        public string LicenseNumber { get; set; } = null!;

        public bool IsVerified { get; set; }

        public ICollection<Vehicle> Vehicles { get; set; } = [];
    }
}
