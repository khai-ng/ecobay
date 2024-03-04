using Kernel.Result;
using Microsoft.AspNetCore.Diagnostics;
using Serilog.Core;
using SharedKernel.Kernel.Dependency;

namespace Identity.API.Extensions
{
    public class InternalExceptionHandler : IExceptionHandler, ISingleton
    {
        Serilog.ILogger _logger;

        public InternalExceptionHandler(Serilog.ILogger logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, 
            Exception exception, 
            CancellationToken cancellationToken)
        {
            var appResult = AppResult.Error(exception.Message);

            _logger
                .ForContext("response", appResult, true)
                .Fatal("Internal server error");

            var httpResult = await appResult.ToHttpResult().ToValueAsync<object>();
            await httpContext.Response.WriteAsJsonAsync(httpResult, cancellationToken);
            return true;
        }
    }
}
