﻿namespace Product.API.Presentation.Configurations
{
    public static class HangfireConfig
    {
        public static void AddHangfireDefaults(this IServiceCollection services, IConfiguration configuration)
        {
            //var mongoDbSetting = configuration.GetSection("MongoConnection:Connection").Get<MongoConnectionOptions>();
            //var mongoClient = new MongoClient(mongoDbSetting!.ConnectionString);

            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseInMemoryStorage()

                //.UseMongoStorage(mongoClient, "hangfire", new MongoStorageOptions
                //{
                //    MigrationOptions = new MongoMigrationOptions
                //    {
                //        MigrationStrategy = new MigrateMongoMigrationStrategy(),
                //        BackupStrategy = new CollectionMongoBackupStrategy(),

                //    },
                //    CheckQueuedJobsStrategy = CheckQueuedJobsStrategy.Poll,
                //    Prefix = "hangfire.mongo",
                //    CheckConnection = true
                //})
            );

            services.AddHangfireServer(serverOptions =>
            {
                serverOptions.ServerName = "Hangfire.Mongo Server";
            });
        }
    }
}
