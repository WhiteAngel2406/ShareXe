using System.Net;

using Microsoft.AspNetCore.Diagnostics;

using ShareXe.Base.Dtos;
using ShareXe.Base.Enums;
using ShareXe.Base.Exceptions;

namespace ShareXe.Base.Middleware
{
    public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var (errorResponse, errorCode) = exception switch
            {
                AppException appEx => HandleAppException(appEx),
                // TODO: Handle more exception types here
                _ => HandleUnknownException(exception)
            };

            httpContext.Response.StatusCode = (int)errorCode.Status;
            await httpContext.Response.WriteAsJsonAsync(errorResponse, cancellationToken);
            return true;
        }

        private (ErrorResponse, ErrorCode) HandleAppException(AppException appException)
        {
            var errorCode = appException.ErrorCode;
            var message = appException.Message;

            if (errorCode.Status >= HttpStatusCode.InternalServerError)
            {
                logger.LogError(appException, "An application error occurred: {Message}", appException.Message);
            }

            return (ErrorResponse.FromErrorCode(errorCode, message), errorCode);
        }

        private (ErrorResponse, ErrorCode) HandleUnknownException(Exception exception)
        {
            logger.LogError(exception, "An unhandled exception occurred.");
            return (ErrorResponse.FromErrorCode(ErrorCode.UnknownError), ErrorCode.UnknownError);
        }
    }
}
