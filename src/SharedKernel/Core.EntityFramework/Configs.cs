using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Trace;

namespace Core.EntityFramework
{
    public static class Configs
    {
        public static IServiceCollection AddEFCoreOpenTelemetry(this IServiceCollection services)
        {
            services.AddOpenTelemetry()
                .WithTracing(tracing =>
                {
                    tracing.AddEntityFrameworkCoreInstrumentation();
                });

            return services;
        }
    }
}
