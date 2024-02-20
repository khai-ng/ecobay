using MediatR;
using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.Logging;
using Serilog;

namespace Identity.Application.Behaviours
{
    public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        private readonly ILogger _logger;
        private IHttpContextAccessor _httpContext;

        public LoggingBehaviour(ILogger logger, IHttpContextAccessor httpContext)
        {
            _logger = logger;
            _httpContext = httpContext;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            //_logger.LogInformation("MediatR handling request: {reqquestName}", typeof(TRequest).Name);
            _logger.Information("MediatR handling request: {reqquestName}", typeof(TRequest).Name);
            var response = await next();
            return response;
        }
    }
}
