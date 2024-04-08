using Core.SharedKernel;

namespace Core.IntergrationEvent
{
    public interface IEventBus
    {
        Task PubliskAsync(IIntergrationEvent evt, CancellationToken cancellationToken = default);

    }
}
