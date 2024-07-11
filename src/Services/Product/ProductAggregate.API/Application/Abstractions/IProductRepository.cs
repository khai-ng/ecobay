using Core.MongoDB.Context;
using Core.MongoDB.Repository;
using ProductAggregate.API.Domain.ProductAggregate;

namespace ProductAggregate.API.Application.Abstractions
{
    public interface IProductRepository: IRepository<ProductItem>, IMongoContextResolver
    {
    }
}
