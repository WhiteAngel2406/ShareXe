using ShareXe.src.Base.Enums;
namespace ShareXe.Base.Exceptions
{
    /// <summary>
    /// Represents an application-specific exception that encapsulates an error code and optional custom message.
    /// </summary>
    /// <remarks>
    /// This exception is used throughout the application to provide structured error handling with predefined error codes.
    /// It allows for both standard error code messages and custom error messages to be specified.
    /// </remarks>
    /// <param name="errorCode">The error code associated with this exception.</param>
    /// <param name="customMessage">An optional custom error message. If not provided, the message from the error code will be used.</param>
    /// <param name="innerException">An optional inner exception that caused this exception.</param>
    public class AppException(ErrorCode errorCode, string? customMessage = null, Exception? innerException = null)
        : Exception(customMessage ?? errorCode.Message, innerException)
    {
        public ErrorCode ErrorCode { get; } = errorCode;
    }
}
