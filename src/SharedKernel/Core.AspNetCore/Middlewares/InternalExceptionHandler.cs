using Core.AspNet.Results;
using Core.AppResults;
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
            
            if (exception.InnerException != null)
                errors.Add(exception.InnerException.Message);
            else
                errors.Add(exception.Message);

            var appResult = AppResult.Error(errors.ToArray());

            _logger
                .ForContext("response", appResult, true)
                .Fatal("Internal server error");

            var httpResult = await appResult.ToHttpResult().ToValueAsync<object>().ConfigureAwait(false);
            await httpContext.Response.WriteAsJsonAsync(httpResult, ct).ConfigureAwait(false);
            return true;
        }
    }
}
