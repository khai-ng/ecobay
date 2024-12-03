using Confluent.Kafka;
using Core.AspNet.Common;
using Core.IntegrationEvents.IntegrationEvents;
using Core.Kafka.OpenTelemetry;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Serilog;

namespace Core.Kafka.Producers
{
    public interface IKafkaProducer : IIntegrationProducer
    {
        Task PublishAsync(string topic, IntegrationEvent @event, CancellationToken ct = default);
        Task PublishAsync(TopicPartitionDto tp, IntegrationEvent @event, CancellationToken ct = default);
    }

    internal class KafkaProducer: IKafkaProducer
    {
        private readonly KafkaProducerConfig _kafkaConfig;
        private readonly ILogger _logger;
        public KafkaProducer(IConfiguration configuration, ILogger logger)
        {
            _kafkaConfig = configuration.GetRequiredConfig<KafkaProducerConfig>("Kafka:Producer")
                ?? throw new ArgumentNullException(nameof(KafkaProducerConfig));
            _logger = logger;
        }

        public Task PublishAsync(IntegrationEvent @event, CancellationToken ct = default)
        {
            if (_kafkaConfig.TopicPartition != null
                && !string.IsNullOrEmpty(_kafkaConfig.TopicPartition.Topic))
                return PublishAsync(_kafkaConfig.TopicPartition, @event, ct);

            if (!string.IsNullOrEmpty(_kafkaConfig.Topic))
                return PublishAsync(_kafkaConfig.Topic, @event, ct);

            return Task.CompletedTask;
        }

        public async Task PublishAsync(string topic, IntegrationEvent @event, CancellationToken ct = default)
        {
            var message = new Message<string, string>
            {
                Key = @event.GetType().Name,
                Value = JsonConvert.SerializeObject(@event)
            };

            using (var activity = KafkaActivityScope.StartProduceActivity(topic, message))
            {
                using var producer = new ProducerBuilder<string, string>(_kafkaConfig.ProducerConfig).Build();
                await Task.Yield();

                var result = await producer.ProduceAsync(_kafkaConfig.Topic, message, ct).ConfigureAwait(false);

                if (activity != null)
                    KafkaActivityScope.UpdateActivityTags(result, activity);

                if (result.Status == PersistenceStatus.Persisted
                    || result.Status == PersistenceStatus.PossiblyPersisted)
                    _logger.ForContext(typeof(KafkaProducer))
                        .ForContext("Host", _kafkaConfig.ProducerConfig.BootstrapServers)
                        .ForContext("Topic", topic)
                        .ForContext("Message", message, true)
                        .Information("Kafka produce message");
            }  
        }

        public async Task PublishAsync(TopicPartitionDto tp, IntegrationEvent @event, CancellationToken ct = default)
        {
            
            using var producer = new ProducerBuilder<string, string>(_kafkaConfig.ProducerConfig).Build();
            await Task.Yield();

            var message = new Message<string, string>
            {
                Key = @event.GetType().Name,
                Value = JsonConvert.SerializeObject(@event)
            };
            var kafkaTp = new TopicPartition(tp.Topic, new Partition(tp.Partition));
            var activity = KafkaActivityScope.StartProduceActivity(kafkaTp, message);

            var result = await producer.ProduceAsync(kafkaTp, message, ct).ConfigureAwait(false);

            if(activity != null)
                KafkaActivityScope.UpdateActivityTags(result, activity);

            if (result.Status == PersistenceStatus.Persisted
                || result.Status == PersistenceStatus.PossiblyPersisted)
                _logger.ForContext(typeof(KafkaProducer))
                    .ForContext("Host", _kafkaConfig.ProducerConfig.BootstrapServers)
                    .ForContext("Topic", tp.Topic)
                    .ForContext("Partition", tp.Partition)
                    .ForContext("Message", message, true)
                    .Information("Kafka produce message");
        }
    }
}
