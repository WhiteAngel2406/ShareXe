using System.ComponentModel.DataAnnotations;

namespace ShareXe.Base.Attributes
{
  /// <summary>
  /// Validates that order parameters conform to the specified format and allowed fields.
  /// </summary>
  /// <remarks>
  /// This attribute validates a list of order parameters where each parameter must follow the format "field:direction".
  /// It ensures that:
  /// <br />
  /// - Each order parameter contains exactly one colon separating the field name and direction
  /// <br />
  /// - The field name is one of the allowed fields (case-insensitive comparison)
  /// <br />
  /// - The direction is either "asc" or "desc" (case-insensitive)
  /// <br />
  /// 
  /// If the value is not a list of strings or the list is empty, validation passes.
  /// </remarks>
  /// <param name="allowedFields">The list of field names that are permitted for ordering operations.</param>
  /// <exception cref="ValidationResult">
  /// Returns a validation error if:
  /// - An order parameter does not contain exactly one colon
  /// - The field name is not in the allowed fields list
  /// - The direction is neither "asc" nor "desc"
  /// </exception>
  public class OrderParamAttribute(params string[] allowedFields) : ValidationAttribute
  {
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
      if (value is not List<string> orders || orders.Count == 0)
      {
        return ValidationResult.Success;
      }

      foreach (var order in orders)
      {
        var parts = order.Split(':');

        if (parts.Length != 2)
        {
          return new ValidationResult($"Order parameter '{order}' is invalid. Expected format is 'field:direction'.");
        }

        var field = parts[0];
        var direction = parts[1].ToLower();

        if (!allowedFields.Contains(field, StringComparer.OrdinalIgnoreCase))
        {
          return new ValidationResult($"Ordering by field '{field}' is not allowed. Allowed fields are: {string.Join(", ", allowedFields)}.");
        }

        if (direction != "asc" && direction != "desc")
        {
          return new ValidationResult($"Order direction '{direction}' is invalid. Allowed directions are 'asc' or 'desc'.");
        }
      }

      return ValidationResult.Success;
    }
  }
}