using Microsoft.AspNetCore.Http;
using Serilog.Core;
using Serilog.Events;

namespace ServiceDefaults
{
    public class HttpContextEnricher : ILogEventEnricher
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpContextEnricher(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var contextObject = HttpContextEnrich(_httpContextAccessor);
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("HttpContext", contextObject, true));
        }

        public static HttpContextLog HttpContextEnrich(IHttpContextAccessor contextAccessor)
        {
            var ctx = contextAccessor.HttpContext;
            if (ctx == null) return null;

            return new HttpContextLog
            {
                IpAddress = ctx.Connection.RemoteIpAddress.ToString(),
                Host = ctx.Request.Host.ToString(),
                Path = ctx.Request.Path.ToString(),
                Method = ctx.Request.Method,
                Protocol = ctx.Request.Protocol,
                //QueryString = ctx.Request.QueryString.ToString(),
                RequestId = ctx.TraceIdentifier.ToString(),
            };
        }
    }

    public class HttpContextLog
    {
        public string IpAddress { get; set; }
        public string Host { get; set; }
        public string Method { get; set; }
        public string Path { get; set; }
        public string Protocol { get; set; }
        //public string QueryString { get; set; }
        public string RequestId { get; set; }
    }
}
