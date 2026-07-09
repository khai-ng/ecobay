namespace Core.Mediator
{
    public interface IMediator
    {
        Task Publish(IRequest request, CancellationToken cancellationToken = default);
        Task<TResponse> Publish<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
    }
}
