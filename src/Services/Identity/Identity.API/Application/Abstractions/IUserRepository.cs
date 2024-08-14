using Core.EntityFramework.Repository;
using Core.Result.Paginations;
using Identity.Application.Services;
using Identity.Domain.Entities.UserAggregate;

namespace Identity.Application.Abstractions
{
    public interface IUserRepository: IRepository<User>
    {
        Task<User?> FindAsync(string userName);
        Task<IEnumerable<string>> GetListRoleAsync(Guid userId);
        Task<IEnumerable<string>> GetListPermissionAsync(Guid userId);
        Task<PagingResponse<User>> GetPagedAsync(GetUserRequest request);
    }
}
