using Confluent.Kafka;
using Core.Kafka.IntegrationTests.Fixtures;
using Core.Kafka.Producers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace Core.Kafka.IntegrationTests
{
    public class ProducerTests : IClassFixture<KafkaFixture>
    {
        private const string _topic = "producer-integration-test";

        private readonly KafkaFixture _kafka;

        public ProducerTests(KafkaFixture kafka)
        {
            _kafka = kafka;
        }

        private IKafkaProducer BuildProducer(string? topic = _topic, AppTopicPartition? topicPartition = null)
        {
            return Host.CreateDefaultBuilder()
                .ConfigureServices(services => services.AddKafkaProducer(cfg =>
                {
                    cfg.Topic = topic;
                    cfg.TopicPartition = topicPartition;
                    cfg.ProducerConfig = new ProducerConfig
                    {
                        BootstrapServers = _kafka.BootstrapServers
                    };
                }))
                .Build()
                .Services
                .GetRequiredService<IKafkaProducer>();
        }

        [Fact]
        public async Task PublishAsync_ByTopic_MessageIsPersisted()
        {
            var producer = BuildProducer();
            var @event = new TestIntegrationEvent("hello-topic");

            await producer.PublishAsync(_topic, @event, TestContext.Current.CancellationToken);

            var consumed = ConsumeOne(_topic);
            Assert.NotNull(consumed);
            Assert.Equal(nameof(TestIntegrationEvent), consumed.Message.Key);

            var deserialized = JsonConvert.DeserializeObject<TestIntegrationEvent>(consumed.Message.Value);
            Assert.Equal(@event.Payload, deserialized!.Payload);
        }

        [Fact]
        public async Task PublishAsync_ByTopicPartition_MessageIsPersisted()
        {
            var topicPartition = new AppTopicPartition(_topic, 0);
            var producer = BuildProducer(topic: null, topicPartition: topicPartition);
            var @event = new TestIntegrationEvent("hello-partition");

            await producer.PublishAsync(topicPartition, @event, TestContext.Current.CancellationToken);

            var consumed = ConsumeOne(_topic);
            Assert.NotNull(consumed);
            Assert.Equal(nameof(TestIntegrationEvent), consumed.Message.Key);

            var deserialized = JsonConvert.DeserializeObject<TestIntegrationEvent>(consumed.Message.Value);
            Assert.Equal(@event.Payload, deserialized!.Payload);
        }

        private ConsumeResult<string, string>? ConsumeOne(string topic)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = _kafka.BootstrapServers,
                GroupId = $"test-verifier-{Guid.NewGuid()}",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false
            };

            using var consumer = new ConsumerBuilder<string, string>(config).Build();
            consumer.Subscribe(topic);

            var result = consumer.Consume(TimeSpan.FromSeconds(15));
            consumer.Unsubscribe();
            return result;
        }
    }
}
