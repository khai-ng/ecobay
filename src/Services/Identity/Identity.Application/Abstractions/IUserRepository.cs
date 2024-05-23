using Core.EntityFramework.Repository;
using Identity.Domain.Entities.UserAggregate;

namespace Identity.Application.Abstractions
{
    public interface IUserRepository: IRepository<User>
    {
        Task<User?> FindAsync(string userName);
        Task<IEnumerable<string>> GetUserRolesAsync(Ulid userId);
        Task<IEnumerable<string>> GetUserPermissionAsync(Ulid userId);
    }
}
