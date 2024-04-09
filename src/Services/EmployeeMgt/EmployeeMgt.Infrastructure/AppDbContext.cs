using Core.Autofac;
using Core.Identity;
using EmployeeMgt.Application.Abstractions;
using EmployeeMgt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace EmployeeMgt.Infrastructure
{
    public class AppDbContext : DbContext, IAppDbContext, IScoped
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Employee> Employees {get;set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder
            .Properties<Ulid>()
            .HaveConversion<UlidToStringConverter>()
            .HaveConversion<UlidToBytesConverter>();
        }
    }
}
