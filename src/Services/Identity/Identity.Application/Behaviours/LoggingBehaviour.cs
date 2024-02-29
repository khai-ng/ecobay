using Kernel.Result;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Core;

namespace Identity.Application.Behaviours
{
    public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : class
        where TResponse : IAppResult
    {
        private readonly ILogger _logger;
        private readonly ILogEventEnricher _logEventEnricher;

        public LoggingBehaviour(ILogger logger, ILogEventEnricher logEventEnricher)
        {
            _logger = logger;
            _logEventEnricher = logEventEnricher;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            _logger
                .ForContext(_logEventEnricher)
                .ForContext("request", request, true)
                .Information("Handling request {requestName}", typeof(TRequest).Name);

            var response = await next();
            if (!(response as IAppResult).IsSuccess)
            {
                _logger
                    .ForContext(_logEventEnricher)
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
