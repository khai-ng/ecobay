using Core.IntegrationEvents.IntegrationEvents;

namespace Core.Kafka.IntegrationTests
{
    public record TestIntegrationEvent(string Payload) : IntegrationEvent;
}
