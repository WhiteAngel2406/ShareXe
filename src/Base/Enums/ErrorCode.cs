using System.Net;

namespace ShareXe.src.Base.Enums
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

        public static readonly ErrorCode Unauthorized = new()
        {
            Status = HttpStatusCode.Unauthorized,
            Code = "UNAUTHORIZED",
            Message = "Authentication is required and has failed or has not yet been provided."
        };

        public static readonly ErrorCode NotFound = new()
        {
            Status = HttpStatusCode.NotFound,
            Code = "NOT_FOUND",
            Message = "The requested resource was not found."
        };

        public static readonly ErrorCode UserBanned = new()
        {
            Status = HttpStatusCode.Forbidden,
            Code = "USER_BANNED",
            Message = "The user is banned from accessing this resource."
        };

        public static readonly ErrorCode UserNotFound = new()
        {
            Status = HttpStatusCode.NotFound,
            Code = "USER_NOT_FOUND",
            Message = "The specified user does not exist."
        };


        private ErrorCode() { }

        public HttpStatusCode Status { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
