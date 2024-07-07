using Core.Result.AppResults;
using Core.Result.Paginations;
using Identity.Domain.Entities.UserAggregate;
using MediatR;

namespace Identity.Application.Services
{
    public class GetUserRequest : PagingRequest, IRequest<AppResult<PagingResponse<User>>>
    { }
}
