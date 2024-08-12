using Core.AspNet.Result;
using Core.Result.AppResults;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace Core.AspNet.Middlewares
{
    public class InternalExceptionHandler : IExceptionHandler
    {
        Serilog.ILogger _logger;

        public InternalExceptionHandler(Serilog.ILogger logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext,
            Exception exception,
            CancellationToken ct)
        {
            List<string> errors = [];
            errors.Add(exception.Message);
            if (exception.InnerException != null)
                errors.Add(exception.InnerException.Message);

            var appResult = AppResult.Error(errors.ToArray());

            _logger
                .ForContext("response", appResult, true)
                .Fatal("Internal server error");

            var httpResult = await appResult.ToHttpResult().ToValueAsync<object>();
            await httpContext.Response.WriteAsJsonAsync(httpResult, ct);
            return true;
        }
    }
}
