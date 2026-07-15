using Core.AppResults;
using Core.Mediator;
using Microsoft.Extensions.Logging;

namespace Core.Behaviors
{
    public class LoggingBehaviour<TRequest, TResponse>(
        ILogger<LoggingBehaviour<TRequest, TResponse>> logger) : IPipeline<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : IAppResult
    {
        public async Task<TResponse> HandleAsync(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling request {ReqData}", request);

            var response = await next();
            if (!(response as IAppResult).IsSuccess)
            {
                logger.LogWarning("Error request {ReqName}", typeof(TRequest).Name);
                return response;
            }

            logger.LogInformation("Handled request {ReqName}", typeof(TRequest).Name);
            return response;
        }
    }
}
