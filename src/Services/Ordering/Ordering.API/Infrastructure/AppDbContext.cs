using Core.Autofac;
using Core.EntityFramework.Context;
using Core.EntityFramework.ServiceDefault;
using Core.SharedKernel;
using Microsoft.EntityFrameworkCore;
using Ordering.API.Domain.OrderAggregate;
using Ordering.API.Domain.OrderAgrregate;
using System.Reflection;

namespace Ordering.API.Infrastructure
{
    public class AppDbContext: BaseDbContext, IScoped
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
