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
                //.WithMetrics(metrics =>
                //{
                //    metrics
                //        .AddAspNetCoreInstrumentation()
                //        .AddHttpClientInstrumentation();
                //})
                .WithTracing(tracing =>
                {
                    tracing
                        .AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation()
                        .AddGrpcClientInstrumentation()
                        .AddConsoleExporter()
                        .AddOtlpExporter();
                });

            //builder.Services.AddOpenTelemetry().UseOtlpExporter();

            return builder;
        }
    }
}
