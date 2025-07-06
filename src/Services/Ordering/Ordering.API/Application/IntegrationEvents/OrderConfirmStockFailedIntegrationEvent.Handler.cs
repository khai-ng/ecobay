
namespace Ordering.API.Application.IntegrationEvents
{
    public class OrderConfirmStockFailedIntegrationEventHandler :
        IIntegrationEventHandler<OrderConfirmStockFailedIntegrationEvent>, ITransient
    {
        private readonly IEventStoreRepository<Order> _orderRepository;

        public OrderConfirmStockFailedIntegrationEventHandler(
            IEventStoreRepository<Order> orderRepository) {
            _orderRepository = orderRepository;
        }
        public Task HandleAsync(OrderConfirmStockFailedIntegrationEvent @event, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
