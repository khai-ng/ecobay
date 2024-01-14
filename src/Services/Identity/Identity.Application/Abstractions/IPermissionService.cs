using Identity.Domain.Entities;

namespace Identity.Application.Abstractions
{
    public interface IPermissionService
    {
        Task<HashSet<string>> GetRolesAsync(Guid userId);
        Task<HashSet<string>> GetPermissionsAsync(Guid userId);
    }
}
