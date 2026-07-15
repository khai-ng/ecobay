namespace Core.Mediator
{
    public class Mediator(IServiceProvider serviceProvider) : IMediator
    {
        public async Task PublishAsync(IRequest request, CancellationToken cancellationToken = default)
        {
            var handlerType = typeof(RequestHandlerBase<>).MakeGenericType(request.GetType());
            var handler = Activator.CreateInstance(handlerType, serviceProvider);
            if (handler is null)
                throw new ArgumentNullException(nameof(handler));

            await ((HandlerBase)handler).HandleAsync(request, cancellationToken);
        }

        public async Task<TResponse> PublishAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            var handlerType = typeof(RequestHandlerBase<,>).MakeGenericType(request.GetType(), typeof(TResponse));
            var handler = Activator.CreateInstance(handlerType, serviceProvider);
            if (handler is null)
                throw new ArgumentNullException(nameof(handler));

            return await ((HandlerBase<TResponse>)handler).HandleAsync(request, cancellationToken);
        }
    }
}
