using Kernel.Result;
using Identity.Domain.Entities;
using MediatR;

namespace Identity.Application.Services
{
    public class GetUserRequest : PagingRequest, IRequest<AppResult<PagingResponse<User>>>
    { }
}
