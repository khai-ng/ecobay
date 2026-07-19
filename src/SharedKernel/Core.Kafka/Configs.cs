using Core.IntegrationEvents.IntegrationEvents;
using Core.Kafka.Consumers;
using Core.Kafka.OpenTelemetry;
using Core.Kafka.Producers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OpenTelemetry;

namespace Core.Kafka
{
    public static class Configs
    {
        public static IServiceCollection AddKafkaProducer(
            this IServiceCollection services,
            Action<KafkaProducerConfigs> configOptions)
        {
            services.Configure(configOptions);
            services.TryAddScoped<IKafkaProducer, KafkaProducer>();
            services.TryAddScoped<IExternalEventProducer>(sp => sp.GetRequiredService<IKafkaProducer>());
            return services;
        }

        public static IServiceCollection AddKafkaConsumer(
            this IServiceCollection services,
            Action<KafkaConsumerConfigs> configOptions)
        {
            services.Configure(configOptions);
            services.AddHostedService<KafkaConsumer>();
            return services;
        }

        /// <summary>
        /// Add <see cref="AddKafkaProducer"/>, <seealso cref="AddKafkaConsumer"/>
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddKafkaCompose(
            this IServiceCollection services,
            Action<KafkaProducerConfigs> producerConfigOptions,
            Action<KafkaConsumerConfigs> consumerConfigOptions)
            => services
                .AddKafkaProducer(producerConfigOptions)
                .AddKafkaConsumer(consumerConfigOptions);

        public static OpenTelemetryBuilder AddKafkaOpenTelemetry(this OpenTelemetryBuilder builder)
        {
            builder
                .WithTracing(tracing =>
                {
                    tracing.AddSource(KafkaActivityScope.ActivitySourceName);
                });

            return builder;
        }
    }
}
