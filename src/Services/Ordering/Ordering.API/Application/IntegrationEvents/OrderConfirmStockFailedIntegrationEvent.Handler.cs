
namespace Ordering.API.Application.IntegrationEvents
{
    public class OrderConfirmStockFailedIntegrationEventHandler :
        IIntegrationEventHandler<OrderConfirmStockFailedIntegrationEvent>, ITransient
    {
        private readonly IEventStoreRepository<Order> _orderRepository;
        private readonly Serilog.ILogger _logger;

        public OrderConfirmStockFailedIntegrationEventHandler(
            IEventStoreRepository<Order> orderRepository,
            Serilog.ILogger logger) {
            _orderRepository = orderRepository;
            _logger = logger;
        }
        public Task HandleAsync(OrderConfirmStockFailedIntegrationEvent @event, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
