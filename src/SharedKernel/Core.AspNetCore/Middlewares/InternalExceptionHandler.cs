using Core.AspNet.Results;
using Core.AppResults;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Core.AspNet.Middlewares
{
    public class InternalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<InternalExceptionHandler> _logger;

        public InternalExceptionHandler(ILogger<InternalExceptionHandler> logger)
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

            _logger.LogCritical(exception, "Internal server error");

            var httpResult = await appResult.ToHttpResult().ToValueAsync<object>().ConfigureAwait(false);
            await httpContext.Response.WriteAsJsonAsync(httpResult, ct).ConfigureAwait(false);
            return true;
        }
    }
}
