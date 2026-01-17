namespace ShareXe.Base.Dtos
{
    /// <summary>
    /// Base data transfer object (DTO) that contains common entity metadata and audit trail information.
    /// </summary>
    /// <remarks>
    /// This DTO is designed to be inherited by other DTOs to provide consistent tracking of entity lifecycle events.
    /// It includes timestamps and user identifiers for creation, modification, and soft deletion operations.
    /// </remarks>
    public class EntityDto
    {
        public Guid Id { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public string? CreatedBy { get; set; }

        public DateTimeOffset? LastModifiedAt { get; set; }
        public string? LastModifiedBy { get; set; }

        public DateTimeOffset? DeletedAt { get; set; }
    }
}
