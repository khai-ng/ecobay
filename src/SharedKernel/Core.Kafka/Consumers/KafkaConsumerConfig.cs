using Confluent.Kafka;

namespace Core.Kafka.Consumers
{
    internal class KafkaConsumerConfig
    {
        public ConsumerConfig ConsumerConfig { get; set; } = default!;
        public string[]? Topics { get; set; } = default;
        public TopicPartition[]? TopicPartitions { get; set; }
    }
}
