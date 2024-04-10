using Core.Result.Paginations;
using Core.SharedKernel;
using Identity.Application.Services;
using Identity.Domain.Entities.UserAggrigate;

namespace Identity.Application.Abstractions
{
    public interface IUserRepository: IRepository<User>
    {
        Task<PagingResponse<User>> GetUsersPagingAsync(GetUserRequest request);
        Task<IEnumerable<string>> GetUserRolesAsync(Ulid userId);
        Task<IEnumerable<string>> GetUserPermissionAsync(Ulid userId);
    }
}
