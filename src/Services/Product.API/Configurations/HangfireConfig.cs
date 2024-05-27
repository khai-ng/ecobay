using Core.MongoDB.Context;
using MongoDB.Driver;
using Hangfire.Mongo;
using Hangfire;
using Hangfire.Mongo.Migration.Strategies;
using Hangfire.Mongo.Migration.Strategies.Backup;

namespace Product.API.Configurations
{
    public static class HangfireConfig
    {
        public static void AddHangfireDefaults(this IServiceCollection services, IConfiguration configuration)
        {
            var mongoDbSetting = configuration.GetSection("ProductDatabase").Get<MongoDbSetting>();
            var mongoClient = new MongoClient(mongoDbSetting.ConnectionString);

            services.AddHangfire(configuration => configuration
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseMongoStorage(mongoClient, "hangfire", new MongoStorageOptions
            {
                MigrationOptions = new MongoMigrationOptions
                {
                    MigrationStrategy = new MigrateMongoMigrationStrategy(),
                    BackupStrategy = new CollectionMongoBackupStrategy()
                },
                Prefix = "hangfire.mongo",
                CheckConnection = true
            }));

            services.AddHangfireServer(serverOptions =>
            {
                serverOptions.ServerName = "Hangfire.Mongo server 1";
            });
        }
    }
}
