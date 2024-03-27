using Core.AspNet.Result;
using Core.Autofac;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace EmployeeManagement.API.Extensions
{
    public class InternalExceptionHandler : IExceptionHandler, ISingleton
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            await httpContext.Response.WriteAsJsonAsync(
                new HttpErrorResult(HttpStatusCode.InternalServerError,
                    exception.Message)
            );
            return true;
        }
    }
}
