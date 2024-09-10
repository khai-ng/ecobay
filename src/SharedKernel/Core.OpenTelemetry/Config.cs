using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Core.OpenTelemetry
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

            //services.AddOpenTelemetryExporters();

            return builder;
        }

        private static IServiceCollection AddOpenTelemetryExporters(this IServiceCollection services) 
        {
            services.AddOpenTelemetry().UseOtlpExporter();

            // builder.Services.AddOpenTelemetry()
            //    .WithMetrics(metrics => metrics.AddPrometheusExporter());

            return services;
        }
    }
}
