using Confluent.Kafka;

namespace Core.Kafka.Consumers
{
    public class KafkaConsumerConfig
    {
        public ConsumerConfig ConsumerConfig { get; set; } = default!;
        public string[]? Topics { get; set; } = default;
    }
}
