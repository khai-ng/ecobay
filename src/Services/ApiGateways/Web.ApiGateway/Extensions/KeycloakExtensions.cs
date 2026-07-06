namespace Web.ApiGateway.Extensions
{
    public static class KeycloakExtensions
    {
        public static void AddKeyCloakSecurity(this SwaggerGenOptions opt, IConfiguration configuration)
        {
            opt.AddSecurityDefinition(nameof(SecuritySchemeType.OAuth2), new OpenApiSecurityScheme()
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    AuthorizationCode = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri(configuration["Keycloak:AuthorizationUrl"]!),
                        TokenUrl = new Uri(configuration["Keycloak:TokenUrl"]!),
                        Scopes = new Dictionary<string, string>
                        {
                            { "openid", "OpenID Connect scope" },
                            { "profile", "User profile" }
                        }
                    }
                }
            });

            opt.AddSecurityRequirement(doc => new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecuritySchemeReference(nameof(SecuritySchemeType.OAuth2), doc),
                    []
                }
            });
        }
    }
}
