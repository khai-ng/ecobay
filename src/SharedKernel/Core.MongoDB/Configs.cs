using Core.MongoDB.Context;
using Core.MongoDB.OpenTelemetry;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OpenTelemetry;

namespace Core.MongoDB
{
    public static class Configs
    {
        public static IServiceCollection AddMongoDbContext<TContext>(
            this IServiceCollection services, 
            Action<MongoContextOptions>? optionsAction,
            ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
            where TContext: MongoContext
        {
            MongoContextOptions mongoDbOptions = new();
            optionsAction?.Invoke(mongoDbOptions);

            services.Add(
                new ServiceDescriptor(typeof(MongoContextOptions), 
                sp => mongoDbOptions, 
                serviceLifetime));

            services.TryAdd(
                new ServiceDescriptor(typeof(TContext), 
                typeof(TContext), 
                serviceLifetime));

            return services;
        }

        public static OpenTelemetryBuilder AddMongoTelemetry(this OpenTelemetryBuilder builder)
        {
            builder
                .WithTracing(tracing =>
                {
                    tracing.AddSource(DiagnosticsActivityEventSubscriber.ActivitySourceName);
                });

            return builder;
        }
    }
}
