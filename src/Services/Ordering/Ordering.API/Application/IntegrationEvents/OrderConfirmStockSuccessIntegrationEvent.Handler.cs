namespace Ordering.API.Application.IntegrationEvents
{
    public class OrderConfirmStockSuccessIntegrationEventHandler : 
        IIntegrationEventHandler<OrderConfirmStockSuccessIntegrationEvent>, ITransient
    {
        private readonly IEventStoreRepository<Order> _orderRepository;
        private readonly Serilog.ILogger _logger;
        public OrderConfirmStockSuccessIntegrationEventHandler(
            IEventStoreRepository<Order> orderRepository,
            Serilog.ILogger logger)
        {
            _orderRepository = orderRepository;
            _logger = logger;
        }

        public async Task HandleAsync(OrderConfirmStockSuccessIntegrationEvent @event, CancellationToken ct = default)
        {
            var order = await _orderRepository.FindAsync(@event.OrderId).ConfigureAwait(false);

            if (order == null)
            {
                _ = NotifyFailedAsync();
                return;
            }

            order.SetStockConfirmed();
            await _orderRepository.UpdateAsync(order.Id, order, order.Version, ct).ConfigureAwait(false);

            _ = NotifySuccessAsync();
        }

        private Task NotifyFailedAsync() 
        {
            _logger.Information("Notify order failed");
            return Task.CompletedTask;
        }

        private Task NotifySuccessAsync()
        {
            _logger.Information("Notify order success");
            return Task.CompletedTask;
        }
    }
}
