using Core.IntegrationEvents.IntegrationEvents;
using Core.Kafka.Consumers;
using Core.Kafka.OpenTelemetry;
using Core.Kafka.Producers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Core.Kafka
{
    public static class Configs
    {
        public static IServiceCollection AddKafkaProducer(this IServiceCollection services)
        {
            services.TryAddScoped<IKafkaProducer, KafkaProducer>();
            return services;
        }

        public static IServiceCollection AddKafkaConsumer(this IServiceCollection services) 
        {
            services.AddSingleton<IEventBus, EventBus>();
            services.AddHostedService<KafkaConsumer>();
            return services;
        }

        /// <summary>
        /// Add <see cref="AddKafkaProducer"/>, <seealso cref="AddKafkaConsumer"/>
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddKafkaCompose(this IServiceCollection services)
            => services
                .AddKafkaProducer()
                .AddKafkaConsumer();

        public static IServiceCollection AddKafkaOpenTelemetry(this IServiceCollection services)
        {
            services.AddOpenTelemetry()
                .WithTracing(tracing =>
                {
                    tracing.AddSource(KafkaActivityScope.ActivitySourceName);
                });

            return services;
        }
    }
}
