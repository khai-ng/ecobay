using Core.EntityFramework.Context;
using Core.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.EntityFramework.IntegrationTests
{
    public class Product: AggregateRoot
    {
        public string Name { get; set; }
        public int Qty { get;set; }
    }

    public class TestDbContext: BaseDbContext
    {
        public TestDbContext(DbContextOptions options) : base(options)
        { }
        public DbSet<Product> Products { get; set; }
    }
}
