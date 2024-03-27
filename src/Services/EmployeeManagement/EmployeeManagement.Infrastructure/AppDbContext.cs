using Core.Autofac;
using EmployeeManagement.Application.Abstractions;
using EmployeeManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace EmployeeManagement.Infrastructure
{
    public class AppDbContext : DbContext, IAppDbContext, IScoped
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Employee> Employees => Set<Employee>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }
    }
}
