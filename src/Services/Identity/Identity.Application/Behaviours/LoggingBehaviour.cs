using Kernel.Result;
using MediatR;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace Identity.Application.Behaviours
{
    public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : class
        where TResponse : IAppResult
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
            _logger
                .ForContext("request", request, true)
                .Information("Handling request {requestName}", typeof(TRequest).Name);

            var response = await next();
            if (!(response as IAppResult).IsSuccess)
            {
                _logger
                    .ForContext("response", response, true)
                    .Error("Error handling request {requestName}", typeof(TRequest).Name);
                return response;
            }

            _logger
                .ForContext("response", response, true)
                .Information("Handled request {requestName}", typeof(TRequest).Name);
            return response;
        }
    }
}
