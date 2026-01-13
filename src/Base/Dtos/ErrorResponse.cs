using System.Text.Json.Serialization;
using ShareXe.Base.Enums;

namespace ShareXe.Base.Dtos
{
  public class ErrorResponse : Response
  {
    public string Code { get; set; } = string.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<ValidationError>? Errors { get; set; }

    protected ErrorResponse()
    {
      Success = false;
    }

    public static ErrorResponse FromErrorCode(ErrorCode errorCode, string? customMessage = null)
    {
      return new ErrorResponse
      {
        Success = false,
        Message = customMessage ?? errorCode.Message,
        Code = errorCode.Code,
      };
    }

    public static ErrorResponse WithValidationErrors(List<ValidationError> validationErrors, string? customMessage = null)
    {
      return new ErrorResponse
      {
        Success = false,
        Message = customMessage ?? ErrorCode.ValidationError.Message,
        Code = ErrorCode.ValidationError.Code,
        Errors = validationErrors
      };
    }

    public class ValidationError
    {
      public string Field { get; set; } = string.Empty;
      public string Message { get; set; } = string.Empty;
    }
  }

}
