using Kernel.Result;
using Microsoft.AspNetCore.Diagnostics;
using SharedKernel.Kernel.Dependency;
using System.Net;

namespace Identity.API.Extensions
{
    public class InternalExceptionHandler : IExceptionHandler, ISingleton
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            await httpContext.Response.WriteAsJsonAsync(
                new HttpErrorResult(HttpStatusCode.InternalServerError,
                    exception.Message
                )
            );
            return true;
        }
    }
}
