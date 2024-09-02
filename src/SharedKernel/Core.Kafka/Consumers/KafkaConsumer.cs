using Confluent.Kafka;
using Core.AspNet.Extensions;
using Core.IntegrationEvents.IntegrationEvents;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Core.Kafka.Consumers
{
    public class KafkaConsumer : IntegrationConsumer
    {
        private readonly KafkaConsumerConfig _kafkaConfig;
        private readonly IEventBus _eventBus;
        private readonly ILogger _logger;

        public KafkaConsumer(IConfiguration configuration, IEventBus eventBus, ILogger logger)
        {
            _kafkaConfig = configuration.GetRequiredConfig<KafkaConsumerConfig>("Kafka:Consumer") 
                ?? throw new ArgumentNullException(nameof(KafkaConsumerConfig));
            _eventBus = eventBus
                ?? throw new ArgumentNullException(nameof(KafkaConsumerConfig));
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken ct)
        {
            using var consumer = new ConsumerBuilder<string, string>(_kafkaConfig.ConsumerConfig).Build();

            if(_kafkaConfig.TopicPartitions != null && _kafkaConfig.TopicPartitions.Length != 0)
                consumer.Assign(_kafkaConfig.TopicPartitions);

            if (_kafkaConfig.Topics != null && _kafkaConfig.Topics.Length != 0)
                consumer.Subscribe(_kafkaConfig.Topics);

            var cancelToken = new CancellationTokenSource();
            while (!ct.IsCancellationRequested)
            {
                try
                {
                    //GH issue: https://github.com/dotnet/extensions/issues/2149#issuecomment-518709751
                    await Task.Yield();
                    var consumerResult = consumer.Consume(cancelToken.Token);
                    var evnentMsg = consumerResult.ToEvent();
                    if(evnentMsg == null)
                    {
                        _logger
                            .ForContext(typeof(KafkaConsumer))
                            .Warning("Couldn't deserialize message type {EventType}", consumerResult.Message.Key);
                        return;
                    }
                    _logger
                        .ForContext(typeof(KafkaConsumer))
                        .Information("Handling mesage {EventType}", consumerResult.Message.Key);
                    await _eventBus.PublishAsync(evnentMsg, ct).ConfigureAwait(false);
                }
                catch (OperationCanceledException)
                {
                    _logger
                        .ForContext(typeof(KafkaConsumer))
                        .Warning("OperationCanceledException");
                    break;
                }
                catch (ConsumeException e)
                {
                    // Consumer errors should generally be ignored (or logged) unless fatal.
                    _logger
                        .ForContext(typeof(KafkaConsumer))
                        .Error($"Consume error: {e.Error.Reason}");

                    if (e.Error.IsFatal)
                    {
                        // https://github.com/edenhill/librdkafka/blob/master/INTRODUCTION.md#fatal-consumer-errors
                        _logger
                            .ForContext(typeof(KafkaConsumer))
                            .Fatal($"Consume fatal: {e.Error.Reason}");
                        break;
                    }
                }
                catch (Exception e)
                {
                    _logger
                        .ForContext(typeof(KafkaConsumer))
                        .Error($"Unexpected error: {e}");
                    break;
                }

            }
        }

    }

}
