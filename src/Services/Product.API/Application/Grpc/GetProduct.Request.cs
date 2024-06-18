using Core.Result.AppResults;
using Core.Result.Paginations;
using MediatR;

namespace Product.API.Application.Grpc
{
    public class GetProductRequest : PagingRequest, IRequest<AppResult<PagingResponse<GetProductResponse>>>
    {
        public string Category { get; set; }
    }
}
