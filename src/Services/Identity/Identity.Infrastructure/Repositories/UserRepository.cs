using Core.Autofac;
using Core.Result.Paginations;
using Core.SharedKernel;
using Identity.Application.Abstractions;
using Identity.Application.Services;
using Identity.Domain.Entities.UserAggrigate;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository, IScoped
    {
        private readonly AppDbContext _context;
        public IUnitOfWork UnitOfWork => _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PagingResponse<User>> GetUsersPagingAsync(GetUserRequest request)
        {
            return await PagingTyped
                .From(request)
                .PagingAsync(_context.Users);
        }

        public async Task<IEnumerable<string>> GetUserRolesAsync(Ulid userId)
        {
            return await _context.Users
                .Include(x => x.Roles)
                .Where(x => x.Id == userId)
                .SelectMany(x => x.Roles)
                .Select(x => x.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<string>> GetUserPermissionAsync(Ulid userId)
        {
            IEnumerable<Permission> rolePermissions = await _context.Users
               .Include(x => x.Roles)
               .ThenInclude(x => x.Permissions)
               .SelectMany(x => x.Roles)
               .SelectMany(x => x.Permissions)
               .ToListAsync();

            IEnumerable<Permission> permissions = await _context.Users
                .Include(x => x.Permissions)
                .Where(x => x.Id == userId)
                .SelectMany(x => x.Permissions)
                .ToListAsync();

            var allPermissions = rolePermissions.Union(permissions);

            return allPermissions.Select(x => x.Name);
        }
    }
}
