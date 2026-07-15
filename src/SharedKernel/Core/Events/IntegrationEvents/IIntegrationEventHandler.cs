using Core.Mediator;

namespace Core.IntegrationEvents.IntegrationEvents
{
    public interface IIntegrationEventHandler<TIntegrationEvent> : IRequestHandler<TIntegrationEvent>    
        where TIntegrationEvent : IntegrationEvent
    { }
}
