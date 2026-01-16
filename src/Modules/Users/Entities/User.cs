using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ShareXe.Base.Attributes;
using ShareXe.Base.Entities;
using ShareXe.Modules.Auth.Entities;

namespace ShareXe.Modules.Users.Entities
{
  [Entity("users")]
  public class User : BaseEntity
  {
    [Required]
    [Unique]
    public Guid AccountId { get; set; }

    [ForeignKey(nameof(AccountId))]
    public virtual Account Account { get; set; } = null!;

    public string? FullName { get; set; }

    public string? Avatar { get; set; }
  }
}