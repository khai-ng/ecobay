using Confluent.Kafka;
using Core.AspNet.Extensions;
using Core.IntegrationEvents;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Core.Kafka.Producers
{
    public class KafkaProducer: IIntegrationProducer
    {
        private readonly ProducerConfig _producerConfig;

        public KafkaProducer(IConfiguration configuration)
        {
            _producerConfig = configuration.GetRequiredConfig<ProducerConfig>("Kafka:ProducerConfig");
        }

        public async Task PublishAsync(IIntegrationProducer evt, CancellationToken cancellationToken = default)
        {

            using var p = new ProducerBuilder<string, string>(_producerConfig).Build();
            await Task.Yield();
            await p.ProduceAsync("my-topic",
                new Message<string, string>
                {
                    Key = evt.GetType().Name,
                    //Value = JsonSerializer.Serialize(evt)
                    Value = JsonConvert.SerializeObject(evt)
                }, cancellationToken).ConfigureAwait(false);
        }
    }
}
