using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using MediatR;

namespace ServiceDefaults
{
    public static class CommonExtensions
    {
        public static WebApplicationBuilder AddServiceDefaults(this WebApplicationBuilder builder)
        {
            //builder.Services.AddDefaultHealthChecks(builder.Configuration);
            //builder.Services.AddDefaultOpenApi(builder.Configuration);
            builder.Services.AddDefaultAuthentication(builder.Configuration);
            builder.Services.AddHttpContextAccessor();
            return builder;
        }

        public static WebApplication UseServiceDefaults(this WebApplication app)
        {
            //var pathBase = app.Configuration["PATH_BASE"];
            //if (!string.IsNullOrEmpty(pathBase))
            //{
            //    app.UsePathBase(pathBase);
            //    app.UseRouting();
            //}

            var identitySection = app.Configuration.GetSection("Identity");

            if (identitySection.Exists())
            {
                app.UseAuthentication();
                app.UseAuthorization();
            }
            //app.UseDefaultOpenApi(app.Configuration);
            app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();
            return app;
        }

        public static IServiceCollection AddDefaultOpenApi(this IServiceCollection services, IConfiguration configuration)
        {
            var openApi = configuration.GetSection("OpenApi");

            if (!openApi.Exists())
            {
                return services;
            }

            return services.AddSwaggerGen(options =>
            {
                var document = openApi.GetRequiredSection("Document");

                var version = document.GetRequiredValue("Version") ?? "v1";

                options.SwaggerDoc(version, new OpenApiInfo
                {
                    Title = document.GetRequiredValue("Title"),
                    Version = version,
                    Description = document.GetRequiredValue("Description")
                });

                var identitySection = configuration.GetSection("Identity");

                if (!identitySection.Exists())
                {
                    return;
                }

                //options.OperationFilter<AuthorizeCheckOperationFilter>();
            });
        }

        public static IServiceCollection AddDefaultAuthentication(this IServiceCollection services, IConfiguration configuration)
        {

            var jwtSection = configuration.GetSection("Jwt");

            if (!jwtSection.Exists())
            {
                return services;
            }

            // prevent from mapping "sub" claim to nameidentifier.
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");

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

        public static IApplicationBuilder UseDefaultOpenApi(this WebApplication app, IConfiguration configuration)
        {
            var openApiSection = configuration.GetSection("OpenApi");

            if (!openApiSection.Exists())
            {
                return app;
            }

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
    }
}
