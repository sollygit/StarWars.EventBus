using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using StarWars.EventHub.Models;
using System;
using System.Net;
using System.Threading.Tasks;

namespace StarWars.EventHub.Services.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await FormatException(context.Response, ex);
            }
        }

        private Task FormatException(HttpResponse response, Exception ex)
        {
            if (ex is NotSupportedException || ex is NotImplementedException)
            {
                response.StatusCode = (int)HttpStatusCode.NotImplemented;
            }
            else if (ex is ServiceException)
            {
                response.StatusCode = (int)(ex as ServiceException).StatusCode;
            }
            else
            {
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            response.ContentType = "application/json";
            return response.WriteAsJsonAsync(new
            {
                error = new
                {
                    message = ex.Message,
                    detail = ex.InnerException?.Message
                }
            });
        }
    }

    public static class ExceptionHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseJsonExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}
