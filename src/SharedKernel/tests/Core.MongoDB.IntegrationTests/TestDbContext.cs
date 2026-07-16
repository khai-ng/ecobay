using Core.MongoDB.Context;
using Core.MongoDB.ServiceDefault;
using MongoDB.Driver;

namespace Core.MongoDB.IntegrationTests
{
    public class Product : AggregateRoot
    {
        public string Name { get; set; }
        public int Qty { get; set; }
    }

    public class TestDbContext : MongoContext
    {
        public TestDbContext(MongoContextOptions options) : base(options) { }

        public IMongoCollection<Product> Products => Collection<Product>();

    }
}
