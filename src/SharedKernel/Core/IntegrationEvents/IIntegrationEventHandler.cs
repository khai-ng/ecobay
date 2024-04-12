namespace Core.IntegrationEvents
{

    public interface IIntegrationEventHandler<in TIntegrationEvent>
        where TIntegrationEvent : IntegrationEvent
    {
        Task Handle(TIntegrationEvent @event, CancellationToken cancellationToken = default);
    }
}
