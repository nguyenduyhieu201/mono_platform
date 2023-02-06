using Mono.SharedLibrary.Exceptions;
using Mono.SharedLibrary.Wrapper;
using System;
using System.Net;
using System.Text.Json;

namespace Mono.API.Middlewares
{
    public class GlobalErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                HttpStatusCode status;
                var stackTrace = string.Empty;
                string message = ex.Message;

                var exceptionType = ex.GetType();

                if (exceptionType == typeof(BadRequestException))
                {
                    status = HttpStatusCode.BadRequest;
                    stackTrace = ex.StackTrace;
                }
                else if (exceptionType == typeof(NotFoundException))
                {
                    status = HttpStatusCode.NotFound;
                    stackTrace = ex.StackTrace;
                }
                else if (exceptionType == typeof(UnauthorizedException))
                {
                    status = HttpStatusCode.Unauthorized;
                    stackTrace = ex.StackTrace;
                }
                else
                {
                    status = HttpStatusCode.InternalServerError;
                    stackTrace = ex.StackTrace;
                }

                var exceptionResult = JsonSerializer.Serialize(Result.Fail(message));
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)status;

                await context.Response.WriteAsync(exceptionResult);
            }
        }
    }
}
