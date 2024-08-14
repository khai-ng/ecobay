using Core.EntityFramework.Identity;
using Core.EntityFramework.ServiceDefault;
using Core.SharedKernel;
using Microsoft.EntityFrameworkCore;

namespace Core.EntityFramework.Context
{
    public abstract class BaseDbContext : DbContext
    {
        public BaseDbContext() { }
        public BaseDbContext(DbContextOptions options) : base(options)
        { }

        //protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        //{
        //    configurationBuilder
        //    .Properties<Ulid>()
        //    .HaveConversion<UlidToStringConverter>()
        //    //.HaveConversion<UlidToBytesConverter>()
        //    ;
        //}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (entityType.ClrType.IsAssignableTo(typeof(Entity<>)))
                {
                    var property = entityType.FindProperty(nameof(Entity.Id))!;
                    entityType.AddKey(property);
                }
                if (entityType.ClrType.IsAssignableTo(typeof(AggregateRoot<>)))
                {
                    entityType.AddIgnored(nameof(AggregateRoot.Events));
                }
            }

            base.OnModelCreating(modelBuilder);
        }
    }
}
