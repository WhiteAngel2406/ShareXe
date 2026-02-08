using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using ShareXe.Base.Attributes;
using ShareXe.Base.Entities;
using ShareXe.Modules.Auth.Entities;
using ShareXe.Modules.DriverProfiles.Entities;
using ShareXe.Modules.Wallets.Entities;

namespace ShareXe.Modules.Users.Entities
{
    [Entity("users")]
    public class User : BaseEntity
    {
        [Required]
        [Unique]
        public Guid AccountId { get; set; }

        [ForeignKey(nameof(AccountId))]
        public Account Account { get; set; } = null!;

        public string? FullName { get; set; }

        public string? Avatar { get; set; }

        public string? Gender { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public DriverProfile? DriverProfile { get; set; }

        public Wallet? Wallet { get; set; } = null!;
    }
}
