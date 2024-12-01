using Confluent.Kafka;

namespace Core.Kafka.Producers
{
    public class KafkaProducerConfig
    {
        public ProducerConfig ProducerConfig { get; set; } = default!;
        public string? Topic { get; set; } = default;
        public TopicPartitionDto? TopicPartition { get; set; }
    }
}
