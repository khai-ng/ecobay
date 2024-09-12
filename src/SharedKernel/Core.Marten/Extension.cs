using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Weasel.Core;
using Marten;
using Microsoft.AspNetCore.Builder;
using OpenTelemetry.Resources;

namespace Core.Marten
{
    public static class Extension
    {
        private const string DefaultConfigKey = "EventStore";

        public static IServiceCollection AddMarten(this IServiceCollection services, IConfiguration configuration)
        {
            var martenOptions = configuration.GetRequiredSection(DefaultConfigKey).Get<MartenConfig>();
            if (martenOptions == null) throw new ArgumentNullException(nameof(martenOptions));

            services.AddMarten(options =>
            {
                options.Connection(martenOptions.ConnectionString);
                options.AutoCreateSchemaObjects = AutoCreate.CreateOrUpdate;

                options.Events.DatabaseSchemaName = martenOptions.WriteSchema;
                //options.DatabaseSchemaName = martenOptions.ReadSchema;

                options.UseSystemTextJsonForSerialization();
            })
            .UseLightweightSessions();

            return services;
        }

        public static WebApplicationBuilder AddMartenOpenTelemetry(this WebApplicationBuilder builder, string? appName = null)
        {
            builder.Services.AddOpenTelemetry()
                .ConfigureResource(resource
                    => resource.AddService(appName ?? builder.Environment.ApplicationName))
                .WithTracing(tracing =>
                {
                    tracing.AddSource("Marten");
                })
                .WithMetrics(metrics =>
                {
                    metrics.AddMeter("Marten");
                });

            return builder;
        }
    }
}
