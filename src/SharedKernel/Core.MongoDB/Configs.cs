using Core.Entities;
using Core.MongoDB.Context;
using Core.MongoDB.OpenTelemetry;
using Microsoft.Extensions.DependencyInjection;
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
                new ServiceDescriptor(typeof(TContext), 
                sp => ActivatorUtilities.CreateInstance(sp, typeof(TContext), mongoDbOptions),
                serviceLifetime));

            // Register UnitOfWork and its interface so consumers can depend on IUnitOfWork
            services.Add(
                new ServiceDescriptor(typeof(IUnitOfWork),
                sp => ActivatorUtilities.CreateInstance(sp, typeof(UnitOfWork), sp.GetRequiredService<TContext>()),
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
