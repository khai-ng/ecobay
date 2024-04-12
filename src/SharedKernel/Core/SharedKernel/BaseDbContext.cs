using Core.Identity;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;

namespace Core.SharedKernel
{
    public abstract class BaseDbContext : DbContext, IUnitOfWork
    {
        public BaseDbContext() { }

        public BaseDbContext(DbContextOptions options) : base(options) { }

        public new async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await this.BulkSaveChangesAsync(cancellationToken: cancellationToken);
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder
            .Properties<Ulid>()
            .HaveConversion<UlidToStringConverter>();
        }
    }
}
