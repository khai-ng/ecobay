using Core.EntityFramework.Context;

namespace Ordering.API.Infrastructure
{
    public class AppDbContext: BaseDbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        { }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
