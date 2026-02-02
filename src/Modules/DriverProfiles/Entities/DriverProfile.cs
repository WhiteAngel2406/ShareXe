using ShareXe.Base.Attributes;
using ShareXe.Base.Entities;
using ShareXe.Modules.Users.Entities;
using ShareXe.src.Modules.VehicleTypes.Entities;

using System.ComponentModel.DataAnnotations;

namespace ShareXe.src.Modules.DriverProfile.Entities
{
    [Entity("driver_profile")]
    public class DriverProfile : BaseEntity
    {
        [Required]
        [Unique]
        public Guid UserId { get; set; }
        public User User { get; set; }
        [Required]
        public String NationalIdCard { get; set; }
        public double RatingScore { get; set; }
        public int TotalTrips { get; set; }
        [Required]
        public String LicenseNumber { get; set; }
        public bool IsVerified { get; set; }
        public ICollection<VehicleType> Vehicles { get; set; }
    }
}
