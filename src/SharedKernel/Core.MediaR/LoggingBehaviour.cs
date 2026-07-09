using Core.AppResults;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Core.MediaR
{
    public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : class
        where TResponse : IAppResult
    {
        private readonly ILogger<LoggingBehaviour<TRequest, TResponse>> _logger;

        public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request,
            RequestHandlerDelegate<TResponse> next,        
            CancellationToken ct)
        {     
            _logger.LogInformation("Handling request {ReqData}", request);

            var response = await next();
            if (!(response as IAppResult).IsSuccess)
            {
                _logger.LogWarning("Error request {ReqName}", typeof(TRequest).Name);
                return response;
            }

            _logger.LogInformation("Handled request {ReqName}", typeof(TRequest).Name);
            return response;
        }
    }
}
