using Microsoft.Extensions.DependencyInjection;

namespace Core.Mediator
{
    public abstract class HandlerBase 
    {
        public abstract Task<object?> Handle(object request, CancellationToken cancellationToken);
    }

    public abstract class HandlerBase<TResponse>
    {
        public abstract Task<TResponse> Handle(object request, CancellationToken cancellationToken);
    }

    public class RequestHandlerBase<TRequest>(IServiceProvider serviceProvider) : HandlerBase
        where TRequest : IRequest
    {
        public Task Handle(IRequest request, CancellationToken cancellationToken)
        {

            async Task<object?> Handler(CancellationToken ct = default)
            {
                var handler = serviceProvider.GetRequiredService<IRequestHandler<TRequest>>();
                await handler.Handle((TRequest)request, ct == default ? cancellationToken : ct);
                return null;
            }

            return serviceProvider
                .GetServices<IPipeline<TRequest, object?>>()
                .Reverse()
                .Aggregate((RequestHandlerDelegate<object?>)Handler,
                    (next, pipeline) => (t) => pipeline.Handle((TRequest)request, next, t == default ? cancellationToken : t))();
        }

        public override Task<object?> Handle(object request, CancellationToken cancellationToken)
        {
            Handle((IRequest)request, cancellationToken);
            return Task.FromResult<object?>(null);
        }
    }

    public class RequestHandlerBase<TRequest, TResponse>(IServiceProvider serviceProvider) : HandlerBase<TResponse>
        where TRequest : IRequest<TResponse>
    {
        public Task<TResponse> Handle(IRequest<TResponse> request, CancellationToken cancellationToken)
        {

            Task<TResponse> Handler(CancellationToken ct = default) => serviceProvider.GetRequiredService<IRequestHandler<TRequest, TResponse>>()
            .Handle((TRequest)request, ct == default ? cancellationToken : ct);

            return serviceProvider
                .GetServices<IPipeline<TRequest, TResponse>>()
                .Reverse()
                .Aggregate((RequestHandlerDelegate<TResponse>)Handler,
                    (next, pipeline) => (t) => pipeline.Handle((TRequest)request, next, t == default ? cancellationToken : t))();
        }

        public override async Task<TResponse> Handle(object request, CancellationToken cancellationToken)
        {
            return await Handle((IRequest<TResponse>)request, cancellationToken);
        }
    }
}
