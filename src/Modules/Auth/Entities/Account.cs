using System.ComponentModel.DataAnnotations;

using ShareXe.Base.Attributes;
using ShareXe.Base.Entities;
using ShareXe.Modules.Users.Entities;
using ShareXe.Base.Enums;

namespace ShareXe.Modules.Auth.Entities
{
    [Entity("accounts")]
    public class Account : BaseEntity
    {
        [Required]
        public string FirebaseUid { get; set; } = null!;

        [MaxLength(255)]
        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        public Role Role { get; set; } = Role.Passenger;

        public bool IsActive { get; set; } = true;

        public Guid? UserId { get; set; }
        public User? User { get; set; }
    }
}
