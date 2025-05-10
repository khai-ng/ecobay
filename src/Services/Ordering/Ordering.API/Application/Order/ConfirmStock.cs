namespace Ordering.API.Application.Services
{
    public class ConfirmStock : IRequestHandler<ConfirmStockCommand, AppResult<string>>, ITransient
    {
        private readonly IEventStoreRepository<Order> _orderRepository;
        private readonly IKafkaProducer _kafkaProducer;

        public ConfirmStock(
            IEventStoreRepository<Order> orderRepository,
            IKafkaProducer kafkaProducer)
        {
            _orderRepository = orderRepository;
            _kafkaProducer = kafkaProducer;
        }

        public async Task<AppResult<string>> Handle(ConfirmStockCommand request, CancellationToken ct)
        {
            var order = await _orderRepository.FindAsync(request.OrderId).ConfigureAwait(false);

            if (order == null)
                return AppResult.Invalid(new ErrorDetail($"Can not find order {request.OrderId}"));

            var orderConfirmStockEvent = 
                new OrderConfirmStockIntegrationEvent(
                    order.Id,
                    order.OrderItems.Select(x => new ProductQty(x.ProductId, x.Qty))
                );

            _ = _kafkaProducer.PublishAsync(orderConfirmStockEvent, ct).ConfigureAwait(false);

            return AppResult.Success("Successful");
        }
    }                                                                                           
}
