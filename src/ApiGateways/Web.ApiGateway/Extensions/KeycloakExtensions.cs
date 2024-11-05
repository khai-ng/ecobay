using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Web.ApiGateway.Extensions
{
    public static class KeycloakExtensions
    {
        public static void AddKeyCloakSecurity(this SwaggerGenOptions opt, string authorizationUrl)
        {
            opt.AddSecurityDefinition("Keycloak", new OpenApiSecurityScheme()
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new()
                {
                    Implicit = new()
                    {
                        AuthorizationUrl = new Uri(authorizationUrl),
                        Scopes = new Dictionary<string, string>
                                {
                                    { "openid", "openid" },
                                    { "profile", "profile" }
                                }
                    }
                }
            });;

            var secRequirement = new OpenApiSecurityRequirement()
                    {
                        {
                            new()
                            {
                                Reference = new()
                                {
                                    Id = "Keycloak",
                                    Type = ReferenceType.SecurityScheme
                                },
                                In = ParameterLocation.Header,
                                Name = "Bearer",
                                Scheme = "Bearer"
                            },
                            []
                        }
                    };
            opt.AddSecurityRequirement(secRequirement);
        }
    }
}
