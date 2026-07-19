using Confluent.Kafka;
using Core.IntegrationEvents.IntegrationEvents;
using Core.Kafka.OpenTelemetry;
using Core.Mediator;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Reflection;

namespace Core.Kafka.Consumers
{
    internal class KafkaConsumer : ExternalEventConsumer
    {
        private readonly KafkaConsumerConfigs _kafkaConfig;
        private readonly IMediator _mediator;
        private readonly ILogger<KafkaConsumer> _logger;
        private readonly Dictionary<string, Type> _eventMap;

        public KafkaConsumer(IOptions<KafkaConsumerConfigs> options, IMediator mediator, ILogger<KafkaConsumer> logger)
        {
            _kafkaConfig = options.Value ?? throw new ArgumentNullException(nameof(KafkaConsumerConfigs));
            _mediator = mediator;
            _logger = logger;

            _eventMap = GetIntegrationEventTypeDictionary() ?? [];
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

                    if (!_eventMap.TryGetValue(consumerResult.Message.Key, out var eventType))
                    {
                        _logger.LogWarning("Couldn't deserialize message type {EventType}", consumerResult.Message.Key);
                        return;
                    }

                    if (JsonConvert.DeserializeObject(consumerResult.Message.Value, eventType) is IntegrationEvent eventMsg)
                    {
                        using (var activity = KafkaActivityScope.StartConsumeActivity(consumerResult, consumer.MemberId))
                        {
                            await _mediator.PublishAsync(eventMsg, ct).ConfigureAwait(false);

                            _logger.LogInformation("Kafka Topic:{Topic} Partition:{Partition} - Consumed mesage {EventType}",
                                consumerResult.Topic, consumerResult.Partition, consumerResult.Message.Key);
                        }

                        return;
                    }
                    _logger.LogWarning("Couldn't deserialize message type {EventType}", consumerResult.Message.Key);
                    return;
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

        private static bool IsSubclassOfRawGeneric(Type generic, Type? toCheck)
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
