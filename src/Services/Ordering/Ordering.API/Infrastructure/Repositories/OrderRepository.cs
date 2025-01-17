namespace Ordering.API.Infrastructure.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository, ITransient
    {
        public OrderRepository(AppDbContext context) : base(context)
        {
        }
    }
}
