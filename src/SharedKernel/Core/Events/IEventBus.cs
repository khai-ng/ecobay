namespace Core.Events
{
    public interface IEventBus
    {
        Task PubliskAsync(IntergrationEvent evt, CancellationToken cancellationToken = default);

    }
}
