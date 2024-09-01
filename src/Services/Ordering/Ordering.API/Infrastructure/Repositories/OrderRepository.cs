using Core.Autofac;
using Core.EntityFramework.Repository;
using Microsoft.EntityFrameworkCore;
using Ordering.API.Application.Common.Abstractions;
using Ordering.API.Domain.OrderAgrregate;

namespace Ordering.API.Infrastructure.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository, ITransient
    {
        private readonly AppDbContext _context;
        public OrderRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public Task<Order?> GetByIdAsync(Guid id)
        {
            return _context.Orders
                .Include(x => x.OrderItems)
                .SingleOrDefaultAsync(x => x.Id == id);
        }
    }
}
