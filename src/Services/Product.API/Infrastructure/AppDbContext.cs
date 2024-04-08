using Core.SharedKernel;
using Microsoft.EntityFrameworkCore;

namespace Product.API.Infrastructure
{
    public class AppDbContext: DbContext, IUnitOfWork
    {

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
