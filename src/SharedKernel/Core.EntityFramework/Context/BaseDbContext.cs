using Core.EntityFramework.Identity;
using Core.SharedKernel;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;

namespace Core.EntityFramework.Context
{
    /// <summary>
    /// Abstracted <see cref="Ulid"/> converter and implemented <see cref="IUnitOfWork"/> 
    /// </summary>
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
            .HaveConversion<UlidToStringConverter>()
            //.HaveConversion<UlidToBytesConverter>()
            ;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
