using Core.Pagination;
using Core.AppResults;
using MediatR;

namespace ProductAggregate.API.Application.Product.GetProduct
{
    public class GetProductRequest : PagingRequest, IRequest<AppResult<PagingResponse<ProductItemDto>>>
    {
        public string Category { get; set; }
    }
}
