namespace Core.IntegrationEvents.IntegrationEvents
{
    public interface IIntegrationProducer
    {
        Task PublishAsync(IntegrationEvent @event, CancellationToken ct = default);
    }
}
