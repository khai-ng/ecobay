using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Ordering.API.Application.Common.Constants;

namespace Ordering.API.Infrastructure
{
    internal class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        private readonly string API_ROUTE = "Ordering.API";

        public AppDbContext CreateDbContext(string[] args)
        {
            // Get environment
            string environment = Environment.GetEnvironmentVariable(AppEnvironment.ASP_ENVIRONMENT)
                ?? AppEnvironment.DEVELOPMENT;

            // Build config
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            // Get connection string
            var builder = new DbContextOptionsBuilder<AppDbContext>();
            var connectionString = config.GetConnectionString(AppEnvironment.DB_SCHEMA)!;
            //builder.UseSqlServer(connectionString);
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 34));
            builder.UseMySql(connectionString, serverVersion);
            return new AppDbContext(builder.Options);
        }
    }
}
