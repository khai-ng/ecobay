using Core.MongoDB.Context;
using Core.MongoDB.Repository;
using MongoDB.Bson;

namespace Product.API.Application.Abstractions
{
    public interface IProductRepository: IRepository<Domain.ProductAggregate.Product>, IMongoContextResolver
    {
        void UpdateProductVersion(IEnumerable<ObjectId> productIds);
    }
}
