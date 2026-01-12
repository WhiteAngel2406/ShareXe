using System.Globalization;
using ShareXe.Base.Enums;
namespace ShareXe.Base.Exceptions
{
    public class AppException(ErrorCode errorCode, string? customMessage = null, Exception? innerException = null)
        : Exception(customMessage ?? errorCode.Message, innerException)
    {
        public ErrorCode ErrorCode { get; } = errorCode;
    }
}
