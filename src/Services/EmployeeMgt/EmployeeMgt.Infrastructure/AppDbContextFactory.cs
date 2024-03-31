using EmployeeMgt.Domain.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace EmployeeMgt.Infrastructure
{
    internal class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {

            // Get environment
            string environment = Environment.GetEnvironmentVariable(AppEnvironment.ASP_ENVIRONMENT)
                ?? AppEnvironment.DEVELOPMENT;

            // Build config
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), AppRoute.API_ROUTE))
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            // Get connection string
            var builder = new DbContextOptionsBuilder<AppDbContext>();
            var connectionString = config.GetConnectionString(AppEnvironment.DB_SCHEMA)!;

            //builder.UseSqlServer(connectionString);
            builder.UseMySQL(connectionString);
            return new AppDbContext(builder.Options);
        }
    }
}
