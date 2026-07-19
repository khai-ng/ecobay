using Confluent.Kafka;

namespace Core.Kafka.Producers
{
    public class KafkaProducerConfigs
    {
        public ProducerConfig ProducerConfig { get; set; } = default!;
        public string? Topic { get; set; } = default;
        public AppTopicPartition? TopicPartition { get; set; }
    }
}
