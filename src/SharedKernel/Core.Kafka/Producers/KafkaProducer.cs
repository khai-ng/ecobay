using Confluent.Kafka;
using Core.AspNet.Extensions;
using Core.IntergrationEvent;
using Core.SharedKernel;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Core.Kafka.Producers
{
    public class KafkaProducer: IExternalProducer
    {
        private readonly ProducerConfig _producerConfig;

        public KafkaProducer(IConfiguration configuration)
        {
            _producerConfig = configuration.GetRequiredConfig<ProducerConfig>("Kafka:ProducerConfig");
        }

        public async Task PublishAsync<T>(T evt, CancellationToken cancellationToken = default)
            where T : IIntergrationEvent
        {

            using var p = new ProducerBuilder<string, string>(_producerConfig).Build();

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
