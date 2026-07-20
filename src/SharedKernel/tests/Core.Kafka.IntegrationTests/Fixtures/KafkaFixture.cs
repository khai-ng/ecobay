using DotNet.Testcontainers.Builders;
using Testcontainers.Kafka;

namespace Core.Kafka.IntegrationTests.Fixtures
{
    public class KafkaFixture : IAsyncLifetime
    {
        private readonly KafkaContainer _container = new KafkaBuilder("confluentinc/cp-kafka:7.6.1")
            .Build();

        public string BootstrapServers => _container.GetBootstrapAddress();

        public async ValueTask InitializeAsync()
        {
            await _container.StartAsync().ConfigureAwait(false);
        }

        public async ValueTask DisposeAsync()
        {
            await _container.DisposeAsync().ConfigureAwait(false);
        }
    }
}
