namespace Core.IntegrationEvents
{
    public interface IIntegrationProducer
    {
        Task PublishAsync(IIntegrationProducer evt, CancellationToken cancellationToken = default);
    }
}
