using Confluent.Kafka;
using Core.AspNet.Extensions;
using Core.IntegrationEvents.IntegrationEvents;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Core.Kafka.Producers
{
    public class KafkaProducer: IIntegrationProducer
    {
        private readonly KafkaProducerConfig _kafkaConfig;

        public KafkaProducer(IConfiguration configuration)
        {
            _kafkaConfig = configuration.GetRequiredConfig<KafkaProducerConfig>("Kafka:Producer") 
                ?? throw new ArgumentNullException(nameof(KafkaProducerConfig));
        }

        public async Task PublishAsync(IntegrationEvent evt, CancellationToken ct = default)
        {
            using var producer = new ProducerBuilder<string, string>(_kafkaConfig.ProducerConfig).Build();
            await Task.Yield();
            var result = await producer.ProduceAsync(_kafkaConfig.Topic,
                new Message<string, string>
                {
                    Key = evt.GetType().Name,
                    //Value = JsonSerializer.Serialize(evt)
                    Value = JsonConvert.SerializeObject(evt)
                }, ct).ConfigureAwait(false);
        }
    }
}
