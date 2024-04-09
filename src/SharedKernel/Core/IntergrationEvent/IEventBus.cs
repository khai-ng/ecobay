using Core.SharedKernel;

namespace Core.IntergrationEvent
{
    public interface IEventBus
    {
        Task PubliskAsync(IEvent evt, CancellationToken cancellationToken = default);

    }
}
