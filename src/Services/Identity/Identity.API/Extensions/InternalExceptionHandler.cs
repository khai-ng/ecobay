using Kernel.Result;
using Microsoft.AspNetCore.Diagnostics;
using SharedKernel.Kernel.Dependency;
using System.Net;

namespace Identity.API.Extensions
{
    public class InternalExceptionHandler : IExceptionHandler, ISingleton
    {
        Serilog.ILogger _logger;

        public InternalExceptionHandler(Serilog.ILogger logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var errorResult = new HttpErrorResult(
                HttpStatusCode.InternalServerError,
                    exception.Message);

            _logger.Fatal(exception, 
                errorResult.Title ?? 
                    "Code: {httpCode}. Message: {errMessage}", HttpStatusCode.InternalServerError, exception.Message);

            await httpContext.Response.WriteAsJsonAsync(errorResult);
            return true;
        }
    }
}
