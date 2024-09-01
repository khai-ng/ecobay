using Hangfire;
using ProductAggregate.API.Application.ConsistentHashing;

namespace ProductAggregate.API.Presentation.Configurations
{
    public static class JobRegistryExtension
    {
        public static void AddHangFireJob(this WebApplication app)
        {
            //RecurringJob.AddOrUpdate<IProductMigrationJob>(
            //    "ProductMigrationJob",
            //    j => j.ProductMigrationJobAsync(),
            //    Cron.Never());
            ////"*/1 * * * *");

            //RecurringJob.AddOrUpdate<IProductMigrationJob>(
            //    "ProductUpdateVirtualJob",
            //    j => j.UpdateVirtualAsync(),
            //    Cron.Never());
        }
    }
}
