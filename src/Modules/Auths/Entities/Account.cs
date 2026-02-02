using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using ShareXe.Base.Attributes;
using ShareXe.Base.Entities;
using ShareXe.Modules.Users.Entities;
using ShareXe.src.Base.Enums;

namespace ShareXe.Modules.Auth.Entities
{
    [Entity("accounts")]
    public class Account : BaseEntity
    {
        public required string FirebaseUid { get; set; }

        [MaxLength(255)]
        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        public Role Role { get; set; } = Role.Passenger;

        public bool IsActive { get; set; } = true;

        [InverseProperty(nameof(User.Account))]
        public virtual User? User { get; set; }
    }
}
