namespace Ordering.API.Application.Abstractions
{
    public interface IOrderRepository : IEventStoreRepository<Order>
    {
        Task<IEnumerable<OrderView>> GetAsync(Guid? Id = null, Guid? BuyerId = null, int? StatusId = null);
    }
}
