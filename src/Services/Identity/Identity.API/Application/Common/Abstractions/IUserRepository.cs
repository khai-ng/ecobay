using Core.EntityFramework.Repository;
using Core.Result.Paginations;
using Identity.Application.Services;
using Identity.Domain.Entities.UserAggregate;

namespace Identity.API.Application.Common.Abstractions
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> FindAsync(string userName);
        Task<IEnumerable<string>> GetListRoleAsync(Guid userId);
        Task<IEnumerable<string>> GetListPermissionAsync(Guid userId);
        Task<PagingResponse<User>> GetPagedAsync(GetUserRequest request);
    }
}
