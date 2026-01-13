using System.Net;

namespace ShareXe.Base.Enums
{
  public class ErrorCode
  {
    public static readonly ErrorCode UnknownError = new()
    {
      Status = HttpStatusCode.InternalServerError,
      Code = "UNKNOWN_ERROR",
      Message = "An unknown error has occurred."
    };

    public static readonly ErrorCode InvalidRequest = new()
    {
      Status = HttpStatusCode.BadRequest,
      Code = "INVALID_REQUEST",
      Message = "Invalid request format."
    };

    public static readonly ErrorCode ValidationError = new()
    {
      Status = HttpStatusCode.BadRequest,
      Code = "VALIDATION_ERROR",
      Message = "Input validation failed."
    };

    public static readonly ErrorCode OperationNotAllowed = new()
    {
      Status = HttpStatusCode.Forbidden,
      Code = "OPERATION_NOT_ALLOWED",
      Message = "This operation is not allowed."
    };


    private ErrorCode() { }

    public HttpStatusCode Status { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
  }
}