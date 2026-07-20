using Confluent.Kafka;
using Core.IntegrationEvents.IntegrationEvents;
using Core.Kafka.Consumers;
using Core.Kafka.IntegrationTests.Fixtures;
using Core.Mediator;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using Newtonsoft.Json;

namespace Core.Kafka.IntegrationTests
{
    public class ConsumerTests : IClassFixture<KafkaFixture>
    {
        private const string Topic = "consumer-integration-test";

        private readonly KafkaFixture _kafka;

        public ConsumerTests(KafkaFixture kafka)
        {
            _kafka = kafka;
        }

        [Fact]
        public async Task Consumer_WhenMessageProduced_MediatorReceivesEvent()
        {
            var @event = new TestIntegrationEvent("hello-consumer");
            ProduceRaw(Topic, @event);

            var mediatorMock = new Mock<IMediator>();
            var received = new TaskCompletionSource<IRequest>(TaskCreationOptions.RunContinuationsAsynchronously);

            mediatorMock
                .Setup(m => m.PublishAsync(It.IsAny<IRequest>(), It.IsAny<CancellationToken>()))
                .Callback<IRequest, CancellationToken>((req, _) => received.TrySetResult(req))
                .Returns(Task.CompletedTask);

            using var host = Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddSingleton(mediatorMock.Object);
                    services.AddKafkaConsumer(cfg =>
                    {
                        cfg.Topics = [Topic];
                        cfg.ConsumerConfig = new ConsumerConfig
                        {
                            BootstrapServers = _kafka.BootstrapServers,
                            GroupId = $"test-consumer-{Guid.NewGuid()}",
                            AutoOffsetReset = AutoOffsetReset.Earliest,
                            EnableAutoCommit = true
                        };
                    });
                })
                .Build();

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            await host.StartAsync(cts.Token);

            var completedTask = await Task.WhenAny(received.Task, Task.Delay(Timeout.Infinite, cts.Token));
            Assert.True(received.Task.IsCompletedSuccessfully, "Mediator did not receive the event within the timeout.");

            var publishedEvent = received.Task.Result as TestIntegrationEvent;
            Assert.NotNull(publishedEvent);
            Assert.Equal(@event.Payload, publishedEvent.Payload);

            await host.StopAsync(CancellationToken.None);
        }

        [Fact]
        public async Task Consumer_WhenUnknownMessageProduced_DoesNotCallMediator()
        {
            var unknownTopic = $"consumer-unknown-{Guid.NewGuid()}";
            ProduceRaw(unknownTopic, "UnknownEventType", "{\"data\":\"irrelevant\"}");

            var mediatorMock = new Mock<IMediator>();

            using var host = Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddSingleton(mediatorMock.Object);
                    services.AddKafkaConsumer(cfg =>
                    {
                        cfg.Topics = [unknownTopic];
                        cfg.ConsumerConfig = new ConsumerConfig
                        {
                            BootstrapServers = _kafka.BootstrapServers,
                            GroupId = $"test-consumer-{Guid.NewGuid()}",
                            AutoOffsetReset = AutoOffsetReset.Earliest,
                            EnableAutoCommit = true
                        };
                    });
                })
                .Build();

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            await host.StartAsync(cts.Token);
            await Task.Delay(TimeSpan.FromSeconds(5), CancellationToken.None);
            await host.StopAsync(CancellationToken.None);

            mediatorMock.Verify(
                m => m.PublishAsync(It.IsAny<IRequest>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }

        private void ProduceRaw(string topic, IntegrationEvent @event)
        {
            ProduceRaw(topic, @event.GetType().Name, JsonConvert.SerializeObject(@event));
        }

        private void ProduceRaw(string topic, string key, string value)
        {
            var config = new ProducerConfig { BootstrapServers = _kafka.BootstrapServers };
            using var producer = new ProducerBuilder<string, string>(config).Build();
            producer.Produce(topic, new Message<string, string> { Key = key, Value = value });
            producer.Flush(TimeSpan.FromSeconds(5));
        }
    }
}
