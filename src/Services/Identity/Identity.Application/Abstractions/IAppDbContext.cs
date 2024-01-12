using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Identity.Application.Abstractions
{
    public interface IAppDbContext
    {
        public DbSet<User> Users { get; }
        public DbSet<Role> Roles { get; }
        public DbSet<Permission> Permissions { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
