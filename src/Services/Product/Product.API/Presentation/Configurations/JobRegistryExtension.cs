﻿namespace Product.API.Presentation.Configurations
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

            RecurringJob.AddOrUpdate<IProductMigrationJob>(
                nameof(IProductMigrationJob.ProcessFileMigrationAsync),
                j => j.ProcessFileMigrationAsync(),
                Cron.Never());
        }
    }
}
