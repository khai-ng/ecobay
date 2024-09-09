using Core.Result.AppResults;
using ProductAggregate.API.Application.Product.Get;
using ProductAggregate.API.Application.Product.Update;
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