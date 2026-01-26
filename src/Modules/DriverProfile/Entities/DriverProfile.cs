using ShareXe.Base.Attributes;
using ShareXe.Base.Enums;
using ShareXe.Modules.Users.Entities;

using System.ComponentModel.DataAnnotations;

namespace ShareXe.src.Modules.DriverProfile.Entities
{
    public class DriverProfile
    {
        [Required]
        [Unique]
        public Guid UserId { get; set; }
        public User User { get; set; }
        public String NationalIdCard { get; set; }
        public double RatingScore { get; set; }
        public int TotalTrips { get; set; }
        public String LicenseNumber { get; set; }
        [Required]
        public VehicleType Vehicle { get; set; }
        public String PlateNumber { get; set; }
        public String Model { get; set; }
        public String Color { get; set; }
        public String ImageURL { get; set; }
        public bool IsActive { get; set; }
        public bool IsVerified { get; set; }
        public ICollection<VehicleType> Vehicles { get; set; }
    }
}
