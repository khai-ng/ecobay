using Core.MongoDB.Context;
using MongoDB.Driver;
using Product.API.Domain.ProductAggregate;

namespace Product.API.Infrastructure
{
    public class AppDbContext: MongoContext
    {
        public AppDbContext(MongoContextOptions options) : base(options) { }

        public IMongoCollection<ProductItem> ProductItems => Collection<ProductItem>();

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
