using Confluent.Kafka;

namespace Core.Kafka.Producers
{
    internal class KafkaProducerConfig
    {
        public ProducerConfig ProducerConfig { get; set; } = default!;
        public string? Topic { get; set; } = default;
    }
}
