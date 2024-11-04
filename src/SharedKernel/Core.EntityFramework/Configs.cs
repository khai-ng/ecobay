using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Trace;

namespace Core.EntityFramework
{
    public static class Configs
    {
        public static WebApplicationBuilder AddEFCoreOpenTelemetry(this WebApplicationBuilder builder)
        {
            builder.Services.AddOpenTelemetry()
                .WithTracing(tracing =>
                {
                    tracing.AddEntityFrameworkCoreInstrumentation();
                });

            return builder;
        }
    }
}
