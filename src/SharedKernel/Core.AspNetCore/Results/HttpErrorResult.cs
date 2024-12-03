using Core.AppResults;
using System.Net;

namespace Core.AspNet.Results
{
    public class HttpErrorResult : HttpResult<HttpErrorResult>
    {
        public HttpErrorResult(IAppResult<HttpErrorResult> appResult) : base(appResult)
        { }

        public HttpErrorResult(
            HttpStatusCode statusCode,
            string message) : base(statusCode, null, message, null)
        { }
        public HttpErrorResult(
            HttpStatusCode statusCode,
            IEnumerable<ErrorDetail>? errors) : base(statusCode, null, null, errors)
        { }

        public HttpErrorResult(
            HttpStatusCode statusCode,
            string? message,
            IEnumerable<ErrorDetail>? errors) : base(statusCode, null, message, errors)
        { }
    }
}
