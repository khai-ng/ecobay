namespace Core.IntegrationEvents.IntegrationEvents
{
    public interface IIntegrationProducer
    {
        Task PublishAsync(IntegrationEvent evt, CancellationToken cancellationToken = default);
    }
}
