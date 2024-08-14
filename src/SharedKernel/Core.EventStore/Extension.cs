using EventStore.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Marten
{
    public static class Extension
    {
        private const string DefaultConfigKey = "EventStore";

        public static IServiceCollection AddEventStoreDB(this IServiceCollection services, IConfiguration configuration)
        {
            var config = configuration.GetRequiredSection(DefaultConfigKey).Get<EventStoreDBConfig>();
            if(config == null) throw new ArgumentNullException(nameof(config));

            services.AddSingleton(new EventStoreClient(EventStoreClientSettings.Create(config.ConnectionString)));
            
            return services;
        }
    }
}
