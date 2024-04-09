using Core.Result.Paginations;
using Core.SharedKernel;
using Identity.Application.Services;
using Identity.Domain.Entities;

namespace Identity.Application.Abstractions
{
    public interface IUserRepository: IRepository<User>
    {
        Task<PagingResponse<User>> GetUsersPaging(GetUserRequest request);
    }
}
