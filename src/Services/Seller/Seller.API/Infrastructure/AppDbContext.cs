using Core.EntityFramework.Context;
using Microsoft.EntityFrameworkCore;
using Seller.API.Domain.SellerAggregate;
using System.Reflection;

namespace Seller.API.Infrastructure
{
    public class AppDbContext: BaseDbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        { }

        public DbSet<SellerItem> SellerItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
