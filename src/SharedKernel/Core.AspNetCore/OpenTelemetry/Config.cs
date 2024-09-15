using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Core.AspNet.OpenTelemetry
{
    public static class Config
    {
        public static WebApplicationBuilder AddDefaultOpenTelemetry(this WebApplicationBuilder builder, string? appName = null)
        {
            builder.Services.AddOpenTelemetry()
                .ConfigureResource(resource
                    => resource.AddService(appName ?? builder.Environment.ApplicationName))
                .WithMetrics(metrics =>
                {
                    metrics
                        .AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation()
                        .AddOtlpExporter();
                })
                .WithTracing(tracing =>
                {
                    tracing
                        .AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation()
                        .AddGrpcClientInstrumentation()
                        .AddOtlpExporter();
                });

            return builder;
        }
    }
}
