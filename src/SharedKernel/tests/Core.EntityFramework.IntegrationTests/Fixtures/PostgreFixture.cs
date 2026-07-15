using Npgsql;
using Testcontainers.PostgreSql;

namespace Core.EntityFramework.Tests.Fixtures
{
    public class PostgreFixture : IAsyncLifetime
    {

        private readonly PostgreSqlContainer container = new PostgreSqlBuilder("postgres:15-alpine")
        //.WithReuse(true)
        .Build();

        public NpgsqlDataSource DataSource { get; private set; } = default!;

        public async ValueTask InitializeAsync()
        {
            await container.StartAsync().ConfigureAwait(false);
            DataSource = new NpgsqlDataSourceBuilder(container.GetConnectionString()).Build();
        }

        public async ValueTask DisposeAsync()
        {
            await DataSource.DisposeAsync().ConfigureAwait(false);
            await container.StopAsync().ConfigureAwait(false);
        }
    }
}
