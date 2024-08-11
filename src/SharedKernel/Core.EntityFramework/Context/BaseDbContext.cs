using Core.EntityFramework.Identity;
using Core.EntityFramework.ServiceDefault;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Core.EntityFramework.Context
{
    public abstract class BaseDbContext : DbContext
    {
        public BaseDbContext() { }
        public BaseDbContext(DbContextOptions options) : base(options)
        { }

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
