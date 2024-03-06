using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Kernel.Behaviors;
using Microsoft.AspNetCore.Diagnostics;
using Serilog.Core;
using Serilog;
using Destructurama;

namespace ServiceDefaults
{
    public static class CommonExtensions
    {
        /// <summary>
        /// Include: Authentication, HttpContextAccessor, Log, Exceptionhandler
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static WebApplicationBuilder AddServiceDefaults(this WebApplicationBuilder builder)
        {
            //builder.Services.AddDefaultHealthChecks(builder.Configuration);
            //builder.Services.AddDefaultOpenApi(builder.Configuration);
            builder.Services.AddDefaultAuthentication(builder.Configuration);
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddDefaultLogging();
            builder.UseDefaultLogging();

            return builder;
        }


        public static WebApplication UseServiceDefaults(this WebApplication app)
        {
            app.UseExceptionHandler(opt => { });
            app.UseDefaultAuthentication();

            app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();
            return app;
        }

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

        public static IServiceCollection AddDefaultLogging(this IServiceCollection services)
        {
            services.AddScoped<IExceptionHandler, InternalExceptionHandler>();
            services.AddScoped<ILogEventEnricher, HttpContextEnricher>();
            return services;
        }

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

        public static WebApplicationBuilder UseDefaultLogging(this WebApplicationBuilder builder)
        {
            Log.Logger = new LoggerConfiguration()
                        .CreateBootstrapLogger();

            builder.Host.UseSerilog((context, serviceProvider, config) =>
            {
                config.ReadFrom.Configuration(context.Configuration);
                config.Destructure.UsingAttributes();
                var enrichers = serviceProvider.GetServices<ILogEventEnricher>();

                if (enrichers is not null)
                    config.Enrich.With(enrichers.ToArray());
            });
            return builder;
        }
    }
}
