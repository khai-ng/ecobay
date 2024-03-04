using SharedKernel.Kernel.Dependency;

namespace Identity.API.Extensions
{
    public class LoggingMiddleware
    {
        //private readonly Serilog.ILogger _logger;
        private readonly RequestDelegate _next;
        public LoggingMiddleware(RequestDelegate next)
        {
            //_logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            Console.WriteLine(context.Request);
            await _next(context);
            Console.WriteLine(context.Response);
        }
    }
}
