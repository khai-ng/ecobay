namespace Core.Mediator
{
    public class Mediator(IServiceProvider serviceProvider) : IMediator
    {
        public async Task Publish(IRequest request, CancellationToken cancellationToken = default)
        {
            var handlerType = typeof(RequestHandlerBase<>).MakeGenericType(request.GetType());
            var handler = Activator.CreateInstance(handlerType, serviceProvider);
            if (handler is null)
                throw new ArgumentNullException(nameof(handler));

            await ((HandlerBase)handler).Handle(request, cancellationToken);
        }

        public async Task<TResponse> Publish<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            var handlerType = typeof(RequestHandlerBase<,>).MakeGenericType(request.GetType(), typeof(TResponse));
            var handler = Activator.CreateInstance(handlerType, serviceProvider);
            if (handler is null)
                throw new ArgumentNullException(nameof(handler));

            return await ((HandlerBase<TResponse>)handler).Handle(request, cancellationToken);
        }
    }
}
