using Core.IntegrationEvents.IntegrationEvents;
using Core.Kafka.Consumers;
using Core.Kafka.Producers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Core.Kafka
{
    public static class Config
    {
        public static IServiceCollection AddKafkaProducer(this IServiceCollection services)
        {
            services.TryAddSingleton<IIntegrationProducer, KafkaProducer>();
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
            => services.AddKafkaProducer().AddKafkaConsumer();
    }
}
