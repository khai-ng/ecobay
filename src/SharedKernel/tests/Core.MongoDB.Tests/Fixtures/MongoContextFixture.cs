using Core.MongoDB.Context;
using Core.Testing.Fixtures;

namespace Core.MongoDB.Tests.Fixtures
{
    public class MongoContextFixture<TContext> : IAsyncLifetime 
        where TContext: MongoContext
    {
        private readonly MongoDbFixture mongoDbFixture = new();

        public TContext DbContext { get; private set; }
        public async ValueTask InitializeAsync()
        {
            await mongoDbFixture.InitializeAsync().ConfigureAwait(false);
            DbContext = (TContext)Activator.CreateInstance(typeof(TContext), mongoDbFixture.MongoConfig)!;
        }

        public async ValueTask DisposeAsync()
        {
            await mongoDbFixture.DisposeAsync().ConfigureAwait(false);
        }
    }
}
