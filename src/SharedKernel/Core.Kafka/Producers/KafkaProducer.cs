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

        public async Task PublishAsync(IntegrationEvent evt, CancellationToken cancellationToken = default)
        {
            using var p = new ProducerBuilder<string, string>(_kafkaConfig.ProducerConfig).Build();
            await Task.Yield();
            await p.ProduceAsync(_kafkaConfig.Topic,
                new Message<string, string>
                {
                    Key = evt.GetType().Name,
                    //Value = JsonSerializer.Serialize(evt)
                    Value = JsonConvert.SerializeObject(evt)
                }, cancellationToken).ConfigureAwait(false);
        }
    }
}
