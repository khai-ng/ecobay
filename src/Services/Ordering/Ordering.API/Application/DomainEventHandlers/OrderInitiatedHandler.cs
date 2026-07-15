namespace Ordering.API.Application.DomainEventHandlers
{
    public class OrderInitiatedHandler : IRequestHandler<OrderInitiated>, ITransient
    {
        public readonly IExternalEventProducer _externalEventProducer;
        private readonly IOrderRepository _orderRepository;

        public OrderInitiatedHandler(IExternalEventProducer externalEventProducer, IOrderRepository orderRepository)
        {
            _externalEventProducer = externalEventProducer;
            _orderRepository = orderRepository;
        }

        public async Task HandleAsync(OrderInitiated notification, CancellationToken cancellationToken = default)
        {
            var order = await _orderRepository.FindAsync(notification.Id).ConfigureAwait(false);

            if (order == null) return;

            var orderConfirmStockEvent =
                new OrderConfirmStockIntegrationEvent(
                    order.Id,
                    order.OrderItems.Select(x => new ProductQty(x.ProductId, x.Qty))
            );

            _ = _externalEventProducer.PublishAsync(orderConfirmStockEvent, cancellationToken).ConfigureAwait(false);

        }
    }
}
