using Core.Entities;
using Core.Marten.OpenTelemetry;
using Core.Marten.Repository;
using Core.Repositories;
using Marten;
using Marten.Events;
using Marten.Events.Daemon.Resiliency;
using Marten.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry;
using Weasel.Core;

namespace Core.Marten
{
    public static class Configs
    {
        private const string DefaultConfigKey = "EventStore";

        public static IServiceCollection AddMarten(
            this IServiceCollection services, 
            IConfiguration configuration,
            Action<StoreOptions>? configure = null)
        {
            var martenConfigs = configuration.GetRequiredSection(DefaultConfigKey).Get<MartenConfigs>();
            if (martenConfigs == null) throw new ArgumentNullException(nameof(martenConfigs));

            var config = services.AddMarten(options =>
            {
                options.Connection(martenConfigs.ConnectionString);
                options.AutoCreateSchemaObjects = AutoCreate.CreateOrUpdate;
                options.Events.DatabaseSchemaName = martenConfigs.WriteSchema;
                options.DatabaseSchemaName = martenConfigs.ReadSchema;
                options.Events.MetadataConfig.CausationIdEnabled = true;
                options.Events.MetadataConfig.CorrelationIdEnabled = true;
                options.Events.MetadataConfig.HeadersEnabled = true;

                options.OpenTelemetry.TrackConnections = TrackLevel.Normal;
                options.OpenTelemetry.TrackEventCounters();

                configure?.Invoke(options);
            })
            .UseLightweightSessions();
            
            if(martenConfigs.EnableDaemon)
                config.AddAsyncDaemon(DaemonMode.Solo);

            return services;
        }

        public static IServiceCollection AddMartenRepository<T>(
            this IServiceCollection services,
            bool withTelemetry = true
        ) where T : AggregateRoot<Guid>
        {
            services.AddScoped<IEventStoreRepository<T>, MartenRepository<T>>();

            if(withTelemetry)
                services.Decorate<IEventStoreRepository<T>>(
                    (inner, sp) => new MartenRepositoryWithTelemetryDecorator<T>(
                        inner,
                        sp.GetRequiredService<IDocumentSession>()
                    )
                );          

            return services;
        }

        public static OpenTelemetryBuilder AddMartenOpenTelemetry(this OpenTelemetryBuilder builder)
        {
            builder
                .WithTracing(tracing =>
                {
                    tracing.AddSource("Marten");
                    tracing.AddSource(MartenActivityScope.ActivitySourceName);
                })
                .WithMetrics(metrics =>
                {
                    metrics.AddMeter("Marten");
                });

            return builder;
        }
    }
}
