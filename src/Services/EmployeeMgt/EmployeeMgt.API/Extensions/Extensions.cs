using EmployeeMgt.Domain.Constants;
using EmployeeMgt.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace EmployeeMgt.API.Extensions
{
    public static class Extensions
    {
        public static IServiceCollection AddDbContexts(this IServiceCollection services, IConfiguration configuration)
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
                options.LogTo(Console.WriteLine);
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

                var serverVersion = new MySqlServerVersion(new Version(8, 0, 34));
                options.UseMySql(connection, serverVersion, sqlOptionsBuilder);
            });

            return services;
        }
    }
}
