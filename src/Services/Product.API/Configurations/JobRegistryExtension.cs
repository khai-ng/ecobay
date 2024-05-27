using Hangfire;
using Product.API.Application.BackgroudJob;

namespace Product.API.Configurations
{
    public static class JobRegistryExtension
    {
        public static void AddHangFireJob(this WebApplication app)
        {
            //using (var scope = app.Services.CreateScope())
            //{
            //    RecurringJob.AddOrUpdate("ProductMigrationJob", 
            //        () => scope.ServiceProvider
            //            .GetRequiredService<IProductMigrationJob>()
            //            .ProductMigrationJobAsync(), 
            //        Cron.Never());
            //}
            //BackgroundJob.Enqueue(() => Console.WriteLine("Hello world from Hangfire!"));

            RecurringJob.AddOrUpdate<IProductMigrationJob>(
                "ProductMigrationJob",
                j => j.ProductMigrationJobAsync(),
                Cron.Never());
        }
    }
}
