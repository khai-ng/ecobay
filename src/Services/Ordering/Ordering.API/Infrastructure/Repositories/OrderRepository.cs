using Core.Autofac;
using Core.EntityFramework.Repository;
using Ordering.API.Application.Abstractions;
using Ordering.API.Domain.OrderAgrregate;

namespace Ordering.API.Infrastructure.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository, IScoped
    {
        public OrderRepository(AppDbContext context) : base(context)
        {
        }
    }
}
