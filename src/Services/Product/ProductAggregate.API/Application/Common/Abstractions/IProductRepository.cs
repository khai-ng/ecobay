using ProductAggregate.API.Domain.ProductAggregate;

namespace ProductAggregate.API.Application.Common.Abstractions
{
    public interface IProductRepository
    {
        Task<AppResult<GetProductRepoResponse>> GetAsync(GetProductRepoRequest request);
        Task<AppResult<IEnumerable<ProductItem>>> GetByIdAsync(GetProductByIdRepoRequest request);
        Task<AppResult> ChannelConfimStockAsync(ConfimStockRepoRequest request);
    }
}