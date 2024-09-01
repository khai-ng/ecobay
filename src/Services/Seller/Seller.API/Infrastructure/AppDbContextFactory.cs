using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Seller.API.Infrastructure
{
    internal class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            // Get environment
            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                ?? "Development";

            // Build config
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            // Get connection string
            var builder = new DbContextOptionsBuilder<AppDbContext>();
            var connectionString = config.GetConnectionString("Default")!;
            //builder.UseSqlServer(connectionString);
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 34));
            builder.UseMySql(connectionString, serverVersion);
            return new AppDbContext(builder.Options);
        }
    }
}
