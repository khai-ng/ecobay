using Microsoft.EntityFrameworkCore;

namespace Core.EntityFramework.IntegrationTests.Fixtures
{
    public class EfCorePostgreFixture<TContext> : IAsyncLifetime
        where TContext : DbContext
    {
        private readonly PostgreFixture postgresFixture = new();
        public TContext DbContext { get; private set; } = default!;

        public async ValueTask InitializeAsync()
        {
            await postgresFixture.InitializeAsync().ConfigureAwait(false);
            var optionsBuilder = new DbContextOptionsBuilder<TContext>()
                .UseNpgsql(postgresFixture.DataSource);

            DbContext = (TContext)Activator.CreateInstance(typeof(TContext), optionsBuilder.Options)!;
            await DbContext.Database.MigrateAsync().ConfigureAwait(false);
        }

        public async ValueTask DisposeAsync()
        {
            await DbContext.DisposeAsync().ConfigureAwait(false);
            await postgresFixture.DisposeAsync().ConfigureAwait(false);
        }

        
    }
}
