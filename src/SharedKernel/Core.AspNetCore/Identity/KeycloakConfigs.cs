using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;

namespace Core.AspNet.Identity
{
    public static class KeycloakConfigs
    {
        public static JwtBearerOptions AddKeyCloakConfigs(this JwtBearerOptions opt, IConfiguration configuration)
        {
            opt.RequireHttpsMetadata = false;
            opt.Audience = configuration["Authentication:Audience"];
            opt.MetadataAddress = configuration["Authentication:MetadataAddress"]!;
            opt.TokenValidationParameters = new()
            {
                ValidIssuer = configuration["Authentication:ValidIssuer"]
            };

            return opt;
        }
    }
}
