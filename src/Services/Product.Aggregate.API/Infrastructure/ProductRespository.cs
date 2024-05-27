using Core.Autofac;
using Core.MongoDB.Context;
using Core.MongoDB.Repository;
using MongoDB.Bson;
using MongoDB.Driver;
using Product.API.Application.Abstractions;

namespace Product.API.Infrastructure
{
    public class ProductRespository : Repository<Domain.ProductAggregate.Product>, IProductRepository, ITransient
    {
        private readonly IMongoContext _context;
        public ProductRespository(IMongoContext context) : base(context)
        {
            _context = context;
            SetCollection("origin_product");
        }

        public void UpdateProductVersion(IEnumerable<ObjectId> productIds)
        {
            //var convertProductIds = productIds.Select(x => ObjectId.Parse(x));
            var filter = Builders<Domain.ProductAggregate.Product>.Filter.In(x => x.Id, productIds);
            var update = Builders<Domain.ProductAggregate.Product>.Update.Set(x => x.Version, 1);
            _context.AddCommand(() => Collection.UpdateManyAsync(filter, update));
        }
    }
}