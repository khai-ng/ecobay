namespace Core.IntegrationEvents.IntegrationEvents
{
    public interface IExternalEventProducer
    {
        Task PublishAsync(IntegrationEvent @event, CancellationToken ct = default);
    }
}
