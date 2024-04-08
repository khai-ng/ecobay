using Core.SharedKernel;

namespace Core.IntergrationEvent
{
    public interface IExternalProducer
    {
        Task PublishAsync<T>(T evt, CancellationToken cancellationToken = default)
            where T : IIntergrationEvent;
    }
}
