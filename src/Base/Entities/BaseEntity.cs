using System.ComponentModel.DataAnnotations;

namespace ShareXe.Base.Entities
{
    /// <summary>
    /// Represents the base entity for all domain entities in the ShareXe application.
    /// This abstract class provides common properties for entity identification, auditing, and soft deletion.
    /// </summary>
    public abstract class BaseEntity
    {
        [Key]
        public Guid Id { get; set; } = Guid.CreateVersion7();

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

        public Guid? CreatedBy { get; set; }

        public DateTimeOffset? LastModifiedAt { get; set; }

        public Guid? LastModifiedBy { get; set; }

        public DateTimeOffset? DeletedAt { get; set; }
    }
}
