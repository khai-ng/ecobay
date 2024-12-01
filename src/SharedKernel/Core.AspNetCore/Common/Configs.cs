using Core.AspNet.Identity;
using Core.AspNet.Middlewares;
using Core.AspNet.OpenTelemetry;
using Destructurama;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Serilog;
using Serilog.Core;
using System.Reflection;

namespace Core.AspNet.Extensions
{
    public static class Configs
    {
        /// <summary>
        /// Included: HttpContextAccessor, Logging, Exceptionhandler
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static WebApplicationBuilder AddServiceDefaults(this WebApplicationBuilder builder)
        {
            builder.AddDefaultLogging();
            builder.AddDefaultOpenTelemetry();
            builder.Services.AddDefaultHealthChecks();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddDefaultExceptionHandler();
            builder.Services.AddDefaultAuthentication(builder.Configuration);

            return builder;
        }

        public static IServiceCollection AddDefaultAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddAuthorization()
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.AddKeyCloakConfigs(configuration);
                });

            return services;
        }

        public static IServiceCollection AddDefaultHealthChecks(this IServiceCollection services)
        {
            services
                .AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy());

            return services;
        }
        public static IServiceCollection AddDefaultExceptionHandler(this IServiceCollection services)
        {
            services.AddExceptionHandler<InternalExceptionHandler>();
            return services;
        }

        public static WebApplicationBuilder AddDefaultLogging(this WebApplicationBuilder builder)
        {
            Log.Logger = new LoggerConfiguration()
                        .CreateBootstrapLogger();

            builder.Host.UseSerilog((context, serviceProvider, config) =>
            {
                var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                IConfiguration configure = new ConfigurationBuilder()
                    .SetBasePath(path!)
                    .AddJsonFile("common.appsettings.json", optional: false, reloadOnChange: true)
                    .AddEnvironmentVariables()
                    .Build();

                config.ReadFrom.Configuration(configure);
                config.Destructure.UsingAttributes();
                var enrichers = serviceProvider.GetServices<ILogEventEnricher>();

                if (enrichers is not null)
                    config.Enrich.With(enrichers.ToArray());
            });

            builder.Services.AddScoped<ILogEventEnricher, HttpContextEnricher>();

            return builder;
        }

        #region UseService
        public static WebApplication UseServiceDefaults(this WebApplication app)
        {
            app.UseExceptionHandler(opt => { });
            app.UseDefaultHealthCheck();
            app.UseDefaultAuthentication();

            return app;
        }

        public static WebApplication UseDefaultSwaggerRedirection(this WebApplication app)
        {
            app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();
            return app;
        }

        public static IApplicationBuilder UseDefaultAuthentication(this WebApplication app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
            
            return app;
        }

        public static IApplicationBuilder UseDefaultHealthCheck(this WebApplication app)
        {
            app.MapHealthChecks("/hc");
            app.MapHealthChecks("/alive", new HealthCheckOptions
            {
                Predicate = r => r.Name.Contains("self")
            });

            return app;
        }
        #endregion
    }
}
