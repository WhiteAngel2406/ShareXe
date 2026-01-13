using System.ComponentModel.DataAnnotations;

namespace ShareXe.Base.Attributes
{
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