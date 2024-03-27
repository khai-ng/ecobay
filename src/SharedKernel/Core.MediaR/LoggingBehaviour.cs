using Core.Result;
using MediatR;
using Serilog;

namespace Core.MediaR
{
    public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : class
        where TResponse : IAppResult
    {
        private readonly ILogger _logger;

        public LoggingBehaviour(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            _logger
                .ForContext("reqData", request, true)
                .Information("Handling request {reqName}", typeof(TRequest).Name);

            var response = await next();
            if (!(response as IAppResult).IsSuccess)
            {
                _logger
                    .ForContext("reqData", response, true)
                    .Warning("Error handling request {reqName}", typeof(TRequest).Name);
                return response;
            }

            _logger
                .ForContext("reqData", response, true)
                .Information("Handled request {reqName}", typeof(TRequest).Name);
            return response;
        }
    }
}
