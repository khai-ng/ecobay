namespace Ordering.API.Application.IntegrationEvents
{
    public class OrderConfirmStockSuccessIntegrationEventHandler : 
        IIntegrationEventHandler<OrderConfirmStockSuccessIntegrationEvent>, ITransient
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IEventStoreRepository<Order> _eventStoreRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly Serilog.ILogger _logger;
        public OrderConfirmStockSuccessIntegrationEventHandler(
            IOrderRepository orderRepository,
            IEventStoreRepository<Order> eventStoreRepository,
            IUnitOfWork unitOfWork,
            Serilog.ILogger logger)
        {
            _orderRepository = orderRepository;
            _eventStoreRepository = eventStoreRepository;
            _unitOfWork = unitOfWork;
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
            _orderRepository.Update(order);

            await _eventStoreRepository.Update(order.Id, order, order.Version, ct: ct).ConfigureAwait(false);
            await _unitOfWork.SaveChangesAsync(ct).ConfigureAwait(false);

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
