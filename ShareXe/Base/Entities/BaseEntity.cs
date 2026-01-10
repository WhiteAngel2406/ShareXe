using System.ComponentModel.DataAnnotations;

namespace ShareXe.Base.Entities
{
    public abstract class BaseEntity
    {
        [Key]
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string? CreatedBy { get; set; }

        public DateTime? LastModifiedAt { get; set; }

        public string? LastModifiedBy { get; set; }

        public DateTime? DeletedAt { get; set; }
    }
}
