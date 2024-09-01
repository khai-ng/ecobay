using Core.IntegrationEvents.IntegrationEvents;

namespace Core.Kafka.Producers
{
    public interface IKafkaProducer : IIntegrationProducer
    {
        Task PublishAsync(string topic, IntegrationEvent @event, CancellationToken ct = default);
        Task PublishAsync(TopicPartition tp, IntegrationEvent @event, CancellationToken ct = default);
    }
}
