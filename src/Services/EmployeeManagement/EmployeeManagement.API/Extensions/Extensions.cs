using EmployeeManagement.Domain.Constants;
using EmployeeManagement.Infrastructure;
using Microsoft.EntityFrameworkCore;
using MySql.EntityFrameworkCore.Infrastructure;

namespace EmployeeManagement.API.Extensions
{
    public static class Extensions
    {
        public static IServiceCollection AddDbContexts(this IServiceCollection services, IConfiguration configuration)
        {
            var sqlOptionsBuilder = (MySQLDbContextOptionsBuilder sqlOptions) =>
            {
                sqlOptions.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName);
                sqlOptions.EnableRetryOnFailure(maxRetryCount: 15,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null);
            };

            var connection = configuration.GetConnectionString(AppEnvironment.DB_SCHEMA)!;
            services.AddDbContextPool<AppDbContext>(options =>
            {
                options.UseMySQL(connection, sqlOptionsBuilder);
            });

            return services;
        }
    }
}
