using Core.Repositories;

namespace Product.API.Application.Common.Abstractions
{
    public interface IProductRepository : ICommandRepository<ProductItem, ObjectId>
    {
        Task<PagingResponse<ProductItem>> GetPagingAsync(GetProductRepoRequest request);
        Task<IEnumerable<ProductItem>> GetAsync(GetProductByIdRepoRequest request);
    }
}
    