using Microsoft.AspNetCore.Http;
using System.Net;

namespace TaskAPI.Web.Configurations.Middlewares {
    public class ExceptionMiddleware {

        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger) {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context) {
            try {
                await _next(context);
            } catch (Exception e) { 
                await HandleExceptionAsync(context, e);
            }
        }

        public async Task HandleExceptionAsync(HttpContext context, Exception ex) {

            context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync((Code: context.Response.StatusCode, Message: "Internal Servcer Error Occured!").ToString());

            _logger.LogError(1,ex,"Exception thrown");
        }

    }
}
