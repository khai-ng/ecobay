using Core.Pagination;
using Core.AppResults;
using Identity.Domain.Entities.UserAggregate;
using MediatR;

namespace Identity.Application.Services
{
    public class GetUserRequest : PagingRequest, IRequest<AppResult<PagingResponse<User>>>
    { }
}
