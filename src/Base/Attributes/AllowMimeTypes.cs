using System.ComponentModel.DataAnnotations;

namespace ShareXe.Base.Attributes
{
    /// <summary>
    /// Validates that an uploaded file has a MIME type that matches one of the allowed patterns.
    /// </summary>
    /// <remarks>
    /// This attribute can be applied to properties of type <see cref="IFormFile"/> to ensure
    /// only files with specified MIME types are accepted. Supports both exact MIME type matching
    /// and wildcard patterns (e.g., "image/*" to allow any image format).
    /// </remarks>
    /// <example>
    /// <code>
    /// [AllowedMimeTypes("image/png", "image/jpeg", "application/*")]
    /// public IFormFile UploadedFile { get; set; }
    /// </code>
    /// </example>
    public class AllowedMimeTypesAttribute(params string[] mimeTypes) : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                var contentType = file.ContentType.ToLower();

                var isAllowed = mimeTypes.Any(pattern =>
                    pattern.EndsWith("/*")
                        ? contentType.StartsWith(pattern.Replace("*", ""))
                        : contentType == pattern);

                if (!isAllowed)
                {
                    return new ValidationResult($"File format {contentType} is not supported.");
                }
            }
            return ValidationResult.Success;
        }
    }
}
