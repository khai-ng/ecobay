using Microsoft.Extensions.Options;
using ProductAggregate.API.Configurations;

namespace Product.API.Configurations
{
    public static class AppExtension
    {
        public static IServiceCollection AddGrpcServices(this IServiceCollection services)
        {
            services.AddGrpcClient<GrpcProduct.Product.ProductClient>((services, options) =>
            {
                var urlConfig = services.GetRequiredService<IOptions<UrlConfiguration>>().Value;
                options.Address = new Uri(urlConfig.GrpcProduct);
            });

            return services;
        }
    }
}
