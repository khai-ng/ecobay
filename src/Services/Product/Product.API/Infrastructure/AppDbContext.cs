using Core.MongoDB.Context;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Product.API.Domain.ProductAggregate;

namespace Product.API.Infrastructure
{
    public class AppDbContext: MongoContext
    {
        public AppDbContext(IOptions<MongoDbOptions> options) : base(options.Value) { }

        public IMongoCollection<ProductItem> ProductItems => Collection<ProductItem>();

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
