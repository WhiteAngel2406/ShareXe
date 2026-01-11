using ShareXe.Base.Dtos;
using ShareXe.Base.Exceptions;
using System.Net;
using System.Text.Json;

namespace ShareXe.Base.Middleware
{
    public class GlobalExceptionHandler
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                var responseModel = new Response<string>() { Succeeded = false, Message = error?.Message };

                switch (error)
                {
                    case AppException e:
                        // 400
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case KeyNotFoundException e:
                        //  404 
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    default:
                        // 500 
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                var result = JsonSerializer.Serialize(responseModel);
                await response.WriteAsync(result);
            }
        }
    }
}
