using Core.MongoDB.Context;
using Core.MongoDB.Repository;
using Core.Result.Paginations;
using MongoDB.Bson;
using Product.API.Application.Grpc;
using Product.API.Domain.ProductAggregate;

namespace Product.API.Application.Abstractions
{
    public interface IProductRepository: IRepository<ProductItem>, IMongoContextResolver
    {
        Task<PagingResponse<ProductItem>> GetAsync(GetProductRequest request);
        Task<IEnumerable<ProductItem>> GetByIdAsync(GetProductByIdRequest request);
    }
}
