using Core.Autofac;
using Identity.Application.Abstractions;
using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Authentication
{
    internal class PermissionService : IPermissionService, IScoped
    {
        private readonly IAppDbContext _context;
        public PermissionService(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<HashSet<string>> GetRolesAsync(Guid userId)
        {
            var roles = await GetUserRolesAsync(userId);
            return roles.Select(x => x.Name).ToHashSet();
        }

        public async Task<HashSet<string>> GetPermissionsAsync(Guid userId)
        {
            var permissions = await GetUserRolePermissionsAsync(userId);
            return permissions.Select(x => x.Name).ToHashSet();
        }

        private async Task<IEnumerable<Permission>> GetUserRolePermissionsAsync(Guid userId)
        {
            var rolePermissions = await _context.Users
               .Include(x => x.Roles)
               .ThenInclude(x => x.Permissions)
               .SelectMany(x => x.Roles)
               .SelectMany(x => x.Permissions)
               .Select(x => x)
               .ToListAsync();

            var userPermissions = await GetUserPermissionsAsync(userId);

            return rolePermissions
                .Union(userPermissions);
        }

        private async Task<IEnumerable<Permission>> GetUserPermissionsAsync(Guid userId)
        {
            return await _context.Users
                .Include(x => x.Permissions)
                .Where(x => x.Id == userId)
                .SelectMany(x => x.Permissions)
                .ToListAsync();
        }

        private async Task<IEnumerable<Role>> GetUserRolesAsync(Guid userId)
        {
            return await _context.Users
                .Include(x => x.Roles)
                .Where(x => x.Id == userId)
                .SelectMany(x => x.Roles)
                .ToListAsync();
        }
    }
}
