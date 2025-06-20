using Confluent.Kafka;
using Core.AspNet.Common;
using Core.IntegrationEvents.IntegrationEvents;
using Core.Kafka.OpenTelemetry;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Reflection;

namespace Core.Kafka.Consumers
{
    internal class KafkaConsumer : IntegrationConsumer
    {
        private readonly KafkaConsumerConfig _kafkaConfig;
        private readonly IEventBus _eventBus;
        private readonly ILogger<KafkaConsumer> _logger;
        private readonly Dictionary<string, Type> eventMap;

        public KafkaConsumer(IConfiguration configuration, IEventBus eventBus, ILogger<KafkaConsumer> logger)
        {
            _kafkaConfig = configuration.GetRequiredConfig<KafkaConsumerConfig>("Kafka:Consumer")
                ?? throw new ArgumentNullException(nameof(KafkaConsumerConfig));
            _eventBus = eventBus;
            _logger = logger;

            eventMap = GetIntegrationEventTypeDictionary() ?? [];
        }

        protected override async Task ExecuteAsync(CancellationToken ct)
        {
            using var consumer = new ConsumerBuilder<string, string>(_kafkaConfig.ConsumerConfig).Build();

            if (_kafkaConfig.TopicPartitions != null && _kafkaConfig.TopicPartitions.Length != 0)
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

                    if (!eventMap.TryGetValue(consumerResult.Message.Key, out var eventType))
                    {
                        _logger.LogWarning("Couldn't deserialize message type {EventType}", consumerResult.Message.Key);
                        return;
                    }

                    var eventMsg = JsonConvert.DeserializeObject(consumerResult.Message.Value, eventType) as IntegrationEvent;
                    if (eventMsg == null) {
                        _logger.LogWarning("Couldn't deserialize message type {EventType}", consumerResult.Message.Key);
                        return;
                    }
                    using (var activity = KafkaActivityScope.StartConsumeActivity(consumerResult, consumer.MemberId))
                    {
                        var isSuccess = await _eventBus.PublishAsync(eventMsg, ct).ConfigureAwait(false);
                        if (isSuccess)
                            _logger.LogInformation("Kafka Topic:{Topic} Partition:{Partition} - Handling mesage {EventType}", 
                                consumerResult.Topic, consumerResult.Partition, consumerResult.Message.Key);
                    }
                }
                catch (OperationCanceledException ex)
                {
                    _logger.LogWarning(ex, "OperationCanceledException");
                    break;
                }
                catch (ConsumeException ex)
                {
                    // Consumer errors should generally be ignored (or logged) unless fatal.
                    _logger.LogError(ex, "Consume error: {Reason}", ex.Error.Reason);

                    if (ex.Error.IsFatal)
                    {
                        // https://github.com/edenhill/librdkafka/blob/master/INTRODUCTION.md#fatal-consumer-errors
                        _logger.LogCritical(ex, "Consume fatal: {Reason}", ex.Error.Reason);
                        break;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unexpected error");
                    break;
                }
            }
        }

        public static Dictionary<string, Type>? GetIntegrationEventTypeDictionary()
        {
            var baseType = typeof(IntegrationEvent<>);

            return Assembly.GetEntryAssembly()?.GetTypes()
                .Where(t => t is { IsClass: true, IsAbstract: false })
                .Where(t => IsSubclassOfRawGeneric(baseType, t))
                .ToDictionary(t => t.Name, t => t);
        }

        private static bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
        {
            while (toCheck != null && toCheck != typeof(object))
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (cur == generic)
                    return true;
                toCheck = toCheck.BaseType;
            }
            return false;
        }
    }
}
