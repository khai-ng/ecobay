using EmployeeMgt.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmployeeMgt.Application.Abstractions
{
    public interface IAppDbContext
    {
        public DbSet<Employee> Employees { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
