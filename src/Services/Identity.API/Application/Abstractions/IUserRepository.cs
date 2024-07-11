using Core.EntityFramework.Repository;
using Identity.Domain.Entities.UserAggregate;

namespace Identity.Application.Abstractions
{
    public interface IUserRepository: IRepository<User>
    {
        Task<User?> FindAsync(string userName);
        Task<IEnumerable<string>> GetListRoleAsync(Ulid userId);
        Task<IEnumerable<string>> GetListPermissionAsync(Ulid userId);
    }
}
