using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Ordering.API.Application.Constants;
using Ordering.API.Infrastructure;

namespace Ordering.API.Presentation.Extensions
{
    public static class DbContextExtension
    {
        public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var sqlOptionsBuilder = (MySqlDbContextOptionsBuilder sqlOptions) =>
            {
                sqlOptions.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName);
                sqlOptions.EnableRetryOnFailure(maxRetryCount: 15,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null);
            };

            var connection = configuration.GetConnectionString(AppEnvironment.DB_SCHEMA)!;

            services.AddDbContextPool<AppDbContext>(options =>
            {
                //options.EnableSensitiveDataLogging(true);
                options.LogTo(Console.WriteLine);
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

                var serverVersion = new MySqlServerVersion(new Version(8, 0, 34));
                options.UseMySql(connection, serverVersion, sqlOptionsBuilder);
            });

            return services;
        }
    }
}
