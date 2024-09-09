using Core.Result.AppResults;
using Core.Result.Paginations;
using MediatR;

namespace ProductAggregate.API.Application.Product.GetProduct
{
    public class GetProductRequest : PagingRequest, IRequest<AppResult<PagingResponse<ProductItemDto>>>
    {
        public string Category { get; set; }
    }
}
