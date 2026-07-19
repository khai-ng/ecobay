using Core.Entities;
using Core.Marten.OpenTelemetry;
using Core.Marten.Repository;
using Core.Repositories;
using JasperFx;
using JasperFx.Events.Daemon;
using JasperFx.OpenTelemetry;
using Marten;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry;

namespace Core.Marten
{
    public static class Configs
    {
        public static IServiceCollection AddDefaultMarten(
            this IServiceCollection services, 
            Action<MartenConfigs> martenAppConfigs,
            Action<StoreOptions>? storeOptions = null)
        {
            ArgumentNullException.ThrowIfNull(martenAppConfigs);
            var appConfigs = new MartenConfigs();
            martenAppConfigs(appConfigs);

            var config = services.AddMarten(options =>
            {
                options.Connection(appConfigs.ConnectionString);
                options.AutoCreateSchemaObjects = AutoCreate.CreateOrUpdate;
                options.Events.DatabaseSchemaName = appConfigs.WriteSchema;
                options.DatabaseSchemaName = appConfigs.ReadSchema;
                options.Events.MetadataConfig.CausationIdEnabled = true;
                options.Events.MetadataConfig.CorrelationIdEnabled = true;
                options.Events.MetadataConfig.HeadersEnabled = true;

                options.OpenTelemetry.TrackConnections = TrackLevel.Normal;
                options.OpenTelemetry.TrackEventCounters();

                storeOptions?.Invoke(options);
            })
            .UseLightweightSessions();
            
            if(appConfigs.EnableDaemon)
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
