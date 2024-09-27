using Core.Pagination;
using Core.Repository;
using MongoDB.Bson;
using Product.API.Application.Product.Get;
using Product.API.Application.Product.GetProducts;
using Product.API.Domain.ProductAggregate;

namespace Product.API.Application.Common.Abstractions
{
    public interface IProductRepository : ICommandRepository<ProductItem, ObjectId>
    {
        Task<PagingResponse<ProductItem>> GetPagingAsync(GetProductRepoRequest request);
        Task<IEnumerable<ProductItem>> GetAsync(GetProductByIdRepoRequest request);
    }
}
