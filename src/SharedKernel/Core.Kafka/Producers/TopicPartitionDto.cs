namespace Core.Kafka.Producers
{
    public class TopicPartitionDto(string  topic, int partition)
    {
        public string Topic { get; set; } = topic;
        public int Partition { get; set; } = partition;
    }
}
