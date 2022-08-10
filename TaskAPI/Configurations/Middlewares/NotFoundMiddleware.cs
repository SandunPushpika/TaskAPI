namespace TaskAPI.Web.Configurations.Middlewares {
    public class NotFoundMiddleware {
        private readonly RequestDelegate _requestDelegate;
        public NotFoundMiddleware(RequestDelegate requestDelegate) {
            _requestDelegate = requestDelegate;
        }
        public async Task InvokeAsync(HttpContext context) { 
            
            await _requestDelegate(context);

            if (context.Response.StatusCode == 404) {
                await context.Response.WriteAsync("Page Not Found!");
            }

        }
    }
}
