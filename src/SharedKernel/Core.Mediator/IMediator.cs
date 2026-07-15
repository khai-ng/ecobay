namespace Core.Mediator
{
    public interface IMediator
    {
        Task PublishAsync(IRequest request, CancellationToken cancellationToken = default);
        Task<TResponse> PublishAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
    }
}
