using Core.Autofac;
using Core.MongoDB.Context;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Product.API.Domain.ProductAggregate;

namespace Product.API.Infrastructure
{
    public class AppDbContext: MongoContext, IScoped
    {
        public AppDbContext(IOptions<MongoDbOptions> options) : base(options.Value) { }

        public IMongoCollection<ProductItem> ProductItems => Collection<ProductItem>("product");
    }
}
