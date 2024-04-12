namespace Core.IntegrationEvents
{
    public interface IEventBus
    {
        Task PubliskAsync(IntegrationEvent evt, CancellationToken cancellationToken = default);

    }
}
