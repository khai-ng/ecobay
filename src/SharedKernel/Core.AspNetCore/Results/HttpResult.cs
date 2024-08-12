using Core.Result.Abstractions;
using Core.Result.AppResults;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json.Serialization;

namespace Core.AspNet.Result
{
    public class HttpResult<T> : HttpResult
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
        public T? Data { get; set; }

        public HttpResult(IAppResult<T> appResult) : base(appResult)
            => Data = appResult.Data;       

        public HttpResult(
            HttpStatusCode statusCode,
            T? data,
            string? message,
            IEnumerable<ErrorDetail>? errors) : base(statusCode, message, errors)       
            => Data = data;
    }

    public class HttpResult : IResult
    {
        private static Dictionary<AppStatusCode, HttpStatusMap> _mappingType = new()
        {
            { AppStatusCode.Ok, new HttpStatusMap(HttpStatusCode.OK)},
            { AppStatusCode.Invalid, new HttpStatusMap(HttpStatusCode.BadRequest) },
            { AppStatusCode.Unauthorized, new HttpStatusMap(HttpStatusCode.Unauthorized) },
            { AppStatusCode.Forbidden, new HttpStatusMap(HttpStatusCode.Forbidden) },
            { AppStatusCode.NotFound, new HttpStatusMap(HttpStatusCode.NotFound) },
            { AppStatusCode.Conflict, new HttpStatusMap(HttpStatusCode.Conflict) },
            { AppStatusCode.Error, new HttpStatusMap(HttpStatusCode.InternalServerError) },
            { AppStatusCode.Unavailable, new HttpStatusMap(HttpStatusCode.ServiceUnavailable)},
        };

        public HttpStatusCode StatusCode { get; private set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Title { get; private set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Type { get; private set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Message { get; private set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IEnumerable<ErrorDetail>? Errors { get; private set; }

        public HttpResult(IAppResult appResult)
        {
            if (!_mappingType.TryGetValue(appResult.Status, out var appStatusMapping))
                throw new NotSupportedException();

            Message = appResult.Message;
            Errors = appResult.Errors;
            StatusCode = appStatusMapping.HttpStatusCode;
            Title = appStatusMapping.Title;
            Type = appStatusMapping.Type;
        }

        public HttpResult(
            HttpStatusCode statusCode,
            string? message,
            IEnumerable<ErrorDetail>? errors)
        {
            var httpStatusMap = new HttpStatusMap(statusCode);

            StatusCode = statusCode;
            Title = httpStatusMap.Title;
            Type = httpStatusMap.Type;
            Message = message;
            Errors = errors;
        }

        public async Task ExecuteAsync(HttpContext httpContext)
        {
            httpContext.Response.StatusCode = (int)StatusCode;
            await httpContext.Response.WriteAsJsonAsync(this);
        }
    }

    internal sealed class HttpStatusMap : TitleTypeMap
    {
        private static readonly Dictionary<HttpStatusCode, TitleTypeMap> _httpStatusMapping = new()
        {
            { HttpStatusCode.OK, new TitleTypeMap( "OK", "https://datatracker.ietf.org/doc/html/rfc7231#section-6.3.1") },
            { HttpStatusCode.BadRequest, new TitleTypeMap( "Bad Request", "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1") },
            { HttpStatusCode.Unauthorized, new TitleTypeMap( "Unauthorized", "https://datatracker.ietf.org/doc/html/rfc7235#section-3.1") },
            { HttpStatusCode.Forbidden, new TitleTypeMap( "Forbidden", "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.3") },
            { HttpStatusCode.NotFound, new TitleTypeMap( "Not Found", "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4") },
            { HttpStatusCode.Conflict, new TitleTypeMap( "Conflict", "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.8") },
            { HttpStatusCode.InternalServerError, new TitleTypeMap( "Internal Server Error", "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1") },
            { HttpStatusCode.ServiceUnavailable, new TitleTypeMap( "Service Unavailable", "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.4") }
        };
        public HttpStatusMap(HttpStatusCode httpStatusCode)
        {
            if (!_httpStatusMapping.TryGetValue(httpStatusCode, out var titleTypeMap))
                throw new NotSupportedException();

            HttpStatusCode = httpStatusCode;
            Title = titleTypeMap.Title;
            Type = titleTypeMap.Type;
        }
        public static HttpStatusMap New(HttpStatusCode httpStatusCode)
        {
            return new HttpStatusMap(httpStatusCode);
        }
        public HttpStatusCode HttpStatusCode { get; private set; }
        
    }

    internal class TitleTypeMap
    {
        public TitleTypeMap() { }
        public TitleTypeMap(string title, string type)
        {
            Title = title;
            Type = type;
        }
        public string Title { get; protected set; }
        public string Type { get; protected set; }
    }
}
