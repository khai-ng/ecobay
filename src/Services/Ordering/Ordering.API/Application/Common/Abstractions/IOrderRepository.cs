namespace Ordering.API.Application.Common.Abstractions
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<Order?> GetByIdAsync(Guid id);
    }
}
