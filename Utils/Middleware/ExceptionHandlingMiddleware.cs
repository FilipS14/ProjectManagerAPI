using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Utils.Middleware.Exceptions;

namespace Utils.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            object response;
            int statusCode;

            switch (exception)
            {
                case NotFoundException _:
                    statusCode = (int)HttpStatusCode.NotFound;
                    response = new 
                    {
                        StatusCode = statusCode,
                        Timestamp = DateTime.UtcNow,
                        Message = "Resource Not Found",
                        Detailed = exception.Message
                    };
                    break;

                case ValidationException validationException:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    response = new 
                    {
                        StatusCode = statusCode,
                        Timestamp = DateTime.UtcNow,
                        Message = "Validation Error",
                        Detailed = validationException.Message,
                        Errors = validationException.Errors
                    };
                    break;

                default:
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    response = new 
                    {
                        StatusCode = statusCode,
                        Timestamp = DateTime.UtcNow,
                        Message = "Internal Server Error",
                        Detailed = exception.Message,
                        StackTrace = exception.StackTrace
                    };
                    break;
            }

            context.Response.StatusCode = statusCode;
            var jsonResponse = JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(jsonResponse);
        }
    }    
}
    