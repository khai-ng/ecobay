namespace Ordering.API.Application.DomainEventHandlers
{
    public class OrderInitiatedHandler : INotificationHandler<OrderInitiated>, ITransient
    {
        public readonly IIntegrationProducer _integrationProducer;
        private readonly IOrderRepository _orderRepository;

        public OrderInitiatedHandler(IIntegrationProducer integrationProducer, IOrderRepository orderRepository)
        {
            _integrationProducer = integrationProducer;
            _orderRepository = orderRepository;
        }

        public async Task Handle(OrderInitiated notification, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.FindAsync(notification.Id).ConfigureAwait(false);

            if (order == null) return;

            var orderConfirmStockEvent =
                new OrderConfirmStockIntegrationEvent(
                    order.Id,
                    order.OrderItems.Select(x => new ProductQty(x.ProductId, x.Qty))
            );

            _ = _integrationProducer.PublishAsync(orderConfirmStockEvent, cancellationToken).ConfigureAwait(false);

        }
    }
}
