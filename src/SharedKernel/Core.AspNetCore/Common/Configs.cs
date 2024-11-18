using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Diagnostics;
using Serilog.Core;
using Serilog;
using Destructurama;
using Core.AspNet.Middlewares;
using System.Reflection;
using Core.AspNet.OpenTelemetry;
using Core.AspNet.Identity;

namespace Core.AspNet.Extensions
{
    public static class Configs
    {
        /// <summary>
        /// Included: HttpContextAccessor, Logging, Exceptionhandler
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static WebApplicationBuilder AddServiceDefaults(this WebApplicationBuilder builder, string? appName = null)
        {
            //builder.Services.AddDefaultHealthChecks(builder.Configuration);
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddDefaultExceptionHandler();
            builder.AddDefaultLogging();
            builder.AddDefaultOpenTelemetry(appName);

            return builder;
        }

        [Obsolete]
        public static IServiceCollection AddDefaultOpenApi(this IServiceCollection services, IConfiguration configuration)
        {
            var openApi = configuration.GetSection("OpenApi");

            if (!openApi.Exists())
                return services;

            return services.AddSwaggerGen(options =>
            {
                var document = openApi.GetRequiredSection("Document");
                var version = document.GetRequiredValue("Version") ?? "v1";
                var identitySection = configuration.GetSection("Identity");

                options.SwaggerDoc(version, new OpenApiInfo
                {
                    Title = document.GetRequiredValue("Title"),
                    Version = version,
                    Description = document.GetRequiredValue("Description")
                });

                if (!identitySection.Exists())
                {
                    return;
                }
            });
        }

        [Obsolete]
        public static IServiceCollection AddDefaultAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSection = configuration.GetSection("Jwt");

            if (!jwtSection.Exists())
                return services;

            // prevent from mapping "sub" claim to nameidentifier.
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");

            services.AddAuthorization();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var jwtOptions = jwtSection.Get<JwtOption>()!;
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.MapInboundClaims = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,                   
                    ValidAudience = jwtOptions.Audience,
                    ValidIssuer = jwtOptions.Issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key))
                };
            });

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
            //app.UseDefaultAuthentication();
            
            return app;
        }

        public static WebApplication UseDefaultSwaggerRedirection(this WebApplication app)
        {
            app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();
            return app;
        }

        [Obsolete]
        public static IApplicationBuilder UseDefaultOpenApi(this WebApplication app, IConfiguration configuration)
        {
            var openApiSection = configuration.GetSection("OpenApi");

            if (!openApiSection.Exists())
                return app;


            app.UseSwaggerUI(setup =>
            {
                var pathBase = configuration["PATH_BASE"];
                var authSection = openApiSection.GetSection("Auth");
                var endpointSection = openApiSection.GetRequiredSection("Endpoint");

                var swaggerUrl = endpointSection["Url"] ?? $"{(!string.IsNullOrEmpty(pathBase) ? pathBase : string.Empty)}/swagger/v1/swagger.json";

                setup.SwaggerEndpoint(swaggerUrl, endpointSection.GetRequiredValue("Name"));

                if (authSection.Exists())
                {
                    setup.OAuthClientId(authSection.GetRequiredValue("ClientId"));
                    setup.OAuthAppName(authSection.GetRequiredValue("AppName"));
                }
            });

            // Add a redirect from the root of the app to the swagger endpoint
            app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();

            return app;
        }

        [Obsolete]
        public static IApplicationBuilder UseDefaultAuthentication(this WebApplication app)
        {
            var identitySection = app.Configuration.GetSection("Jwt");
            if (identitySection.Exists())
            {
                app.UseAuthentication();
                app.UseAuthorization();
            }
            return app;
        }
        #endregion
    }
}
