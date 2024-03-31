using Core.Events.External;
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
            services.TryAddSingleton<IExternalProducer, KafkaProducer>();
            return services;
        }

        public static IServiceCollection AddKafkaConsumer(this IServiceCollection services) 
        {
            services.AddHostedService<KafkaConsumer>();
            return services;
        }


    }
}
