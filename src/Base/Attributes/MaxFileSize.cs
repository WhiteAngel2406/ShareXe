using System.ComponentModel.DataAnnotations;

namespace ShareXe.Base.Attributes
{
    /// <summary>
    /// A validation attribute that enforces a maximum file size constraint on uploaded files.
    /// </summary>
    /// <remarks>
    /// This attribute validates that an <see cref="IFormFile"/> does not exceed the specified maximum size.
    /// The maximum size is specified in bytes and is converted to megabytes for user-facing error messages.
    /// </remarks>
    /// <param name="maxFileSize">The maximum allowed file size in bytes.</param>
    /// <example>
    /// <code>
    /// [MaxFileSize(5242880)] // 5MB
    /// public IFormFile UploadedDocument { get; set; }
    /// </code>
    /// </example>
    public class MaxFileSizeAttribute(int maxFileSize) : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is IFormFile file && file.Length > maxFileSize)
                return new ValidationResult($"Max file size is {maxFileSize / 1024 / 1024}MB.");

            return ValidationResult.Success;
        }
    }
}
