namespace ShareXe.Base.Dtos
{
    public class BaseDto
    {
        public int Id { get; set; }

        public string CreatedAt { get; set; } = string.Empty;

        public string? CreatedBy { get; set; }

        public string? LastModifiedAt { get; set; }

        public string? LastModifiedBy { get; set; }

        public string? DeletedAt { get; set; }

    }
}
