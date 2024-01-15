using Web.ApiGateway.Constants;

namespace Web.ApiGateway.Extensions
{
    public static class Extensions
    {
        public static IServiceCollection AddReverseProxy(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddReverseProxy().LoadFromConfig(configuration.GetRequiredSection(ConfigConstants.YARP_CONFIG));
            return services;
        }
    }
}
