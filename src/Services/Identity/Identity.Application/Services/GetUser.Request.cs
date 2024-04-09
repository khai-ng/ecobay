using Core.Result.AppResults;
using Core.Result.Paginations;
using Identity.Domain.Entities;
using MediatR;

namespace Identity.Application.Services
{
    public class GetUserRequest : PagingRequest, IRequest<AppResult<PagingResponse<User>>>
    { }
}
