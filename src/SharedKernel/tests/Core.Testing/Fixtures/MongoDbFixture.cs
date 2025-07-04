using Core.MongoDB.Context;
using Testcontainers.MongoDb;

namespace Core.Testing.Fixtures
{
    public class MongoDbFixture : IAsyncLifetime
    {
        private readonly MongoDbContainer container = new MongoDbBuilder()
            .WithUsername("mongo")
            .WithPassword("mongo")
            //.WithReuse(true)
            .Build();

        public MongoContextOptions MongoConfig { get; set; } = default!;
        public async Task InitializeAsync()
        {
            await container.StartAsync().ConfigureAwait(false);

            var uri = new Uri(container.GetConnectionString());
            var hostPort = uri.Authority;
            var query = uri.Query.TrimStart('?');
            var conStr = $"mongodb://mongo:mongo@{hostPort}/testDb?authSource=admin";
            if (!string.IsNullOrEmpty(query))          
                conStr += "&" + query;
            
            MongoConfig = new MongoContextOptions() 
            { 
                ConnectionString = conStr
            };
        }

        public async Task DisposeAsync()
        {
            await container.DisposeAsync().ConfigureAwait(false);
        }
    }
}
