using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ServiceDefaults;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace ServiceDefaults
{
    public static class CommonExtensions
    {
        public static WebApplicationBuilder AddServiceDefaults(this WebApplicationBuilder builder)
        {
            //builder.Services.AddDefaultHealthChecks(builder.Configuration);
            builder.Services.AddDefaultOpenApi(builder.Configuration);
            builder.Services.AddDefaultAuthentication(builder.Configuration);
            builder.Services.AddHttpContextAccessor();
            return builder;
        }

        public static WebApplication UseServiceDefaults(this WebApplication app)
        {
            var identitySection = app.Configuration.GetSection("Identity");

            if (identitySection.Exists())
            {
                app.UseAuthentication();
                app.UseAuthorization();
            }
            app.UseDefaultOpenApi(app.Configuration);

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
                /// {
                ///   "OpenApi": {
                ///     "Document": {
                ///         "Title": ..
                ///         "Version": ..
                ///         "Description": ..
                ///     }
                ///   }
                /// }
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
                    // No identity section, so no authentication open api definition
                    return;
                }

                // {
                //   "Identity": {
                //     "ExternalUrl": "http://identity",
                //     "Scopes": {
                //         "basket": "Basket API"
                //      }
                //    }
                // }

                var identityUrlExternal = identitySection["ExternalUrl"] ?? identitySection.GetRequiredValue("Url");
                var scopes = identitySection.GetRequiredSection("Scopes").GetChildren().ToDictionary(p => p.Key, p => p.Value);

                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows()
                    {
                        Implicit = new OpenApiOAuthFlow()
                        {
                            AuthorizationUrl = new Uri($"{identityUrlExternal}/connect/authorize"),
                            TokenUrl = new Uri($"{identityUrlExternal}/connect/token"),
                            Scopes = scopes,
                        }
                    }
                });

                //options.OperationFilter<AuthorizeCheckOperationFilter>();
            });
        }

        public static IServiceCollection AddDefaultAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            // {
            //   "Identity": {
            //     "Url": "http://identity",
            //     "Audience": "basket"
            //    }
            // }

            var identitySection = configuration.GetSection("Identity");

            if (!identitySection.Exists())
            {
                return services;
            }

            // prevent from mapping "sub" claim to nameidentifier.
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var jwtOptions = configuration.GetSection("Jwt").Get<JwtOption>()!;
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

            app.UseSwagger();
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
