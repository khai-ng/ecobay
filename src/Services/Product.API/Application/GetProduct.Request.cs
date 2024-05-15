using Core.Result.AppResults;
using Core.Result.Paginations;
using MediatR;

namespace Product.API.Application
{
    public class GetProductRequest: PagingRequest, IRequest<AppResult<PagingResponse<Domain.Product>>>
    {
    }
}
