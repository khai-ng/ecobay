namespace Core.IntegrationEvents.IntegrationEvents
{

    public interface IIntegrationEventHandler<in TIntegrationEvent>
        where TIntegrationEvent : IntegrationEvent
    {
        Task HandleAsync(TIntegrationEvent @event, CancellationToken cancellationToken = default);
    }
}
