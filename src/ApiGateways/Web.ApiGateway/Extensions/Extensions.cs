using GrpcEmployee;
using Microsoft.Extensions.Options;
using Web.ApiGateway.Configurations;
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
        public static IServiceCollection AddGrpcServices(this IServiceCollection services)
        {
            services.AddGrpcClient<Employee.EmployeeClient>((services, options) =>
            {
                var employeeApi = services.GetRequiredService<IOptions<UrlsConfiguration>>().Value.GrpcEmployee;
                options.Address = new Uri(employeeApi);
            });

            return services;
        }
    }
}
