using Core.AppResults;
using System.Net;

namespace Core.AspNet.Results
{
    /// <summary>
    /// User for display result format only. Don't create instant or inherit
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class HttpResultTyped<T>
    {
        private HttpResultTyped() { }

        public HttpStatusCode StatusCode { get; }

        public string? Title { get; }

        public string? Type { get; }

        public T? Data { get; }

        public string? Message { get; }

        public IEnumerable<ErrorDetail>? Errors { get; }

    }
}
