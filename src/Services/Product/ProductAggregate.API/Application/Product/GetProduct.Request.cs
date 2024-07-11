using Core.Result.AppResults;
using Core.Result.Paginations;
using MediatR;

namespace ProductAggregate.API.Application.Product
{
    public class GetProductRequest : PagingRequest, IRequest<AppResult<PagingResponse<GetProductResponse>>>
    {
        public string Category { get; set; }
    }
}
