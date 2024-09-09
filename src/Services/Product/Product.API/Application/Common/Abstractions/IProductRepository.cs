using Core.MongoDB.Context;
using Core.MongoDB.Repository;
using Core.Result.Paginations;
using MongoDB.Bson;
using Product.API.Application.Product.GetProducts;
using Product.API.Domain.ProductAggregate;

namespace Product.API.Application.Common.Abstractions
{
    public interface IProductRepository : IRepository<ProductItem>, IMongoContextResolver
    {
        Task<PagingResponse<ProductItem>> GetPagingAsync(GetProductRepoRequest request);
        Task<IEnumerable<ProductItem>> GetAsync(IEnumerable<ObjectId> ids);
    }
}
