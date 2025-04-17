namespace Ordering.API.Application.Services
{
    public class ConfirmStock : IRequestHandler<ConfirmStockRequest, AppResult<string>>, ITransient
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IKafkaProducer _kafkaProducer;

        public ConfirmStock(
            IOrderRepository orderRepository,
            IKafkaProducer kafkaProducer)
        {
            _orderRepository = orderRepository;
            _kafkaProducer = kafkaProducer;
        }

        public async Task<AppResult<string>> Handle(ConfirmStockRequest request, CancellationToken ct)
        {
            var order = await _orderRepository.FindAsync(request.OrderId, 
                x => new
                {
                    x.Id,
                    x.OrderStatus,
                    ProductUnits = x.OrderItems.Select(x => new ProductUnit(x.ProductId, x.Qty))
                })
                .ConfigureAwait(false);

            if (order == null)
                return AppResult.Invalid(new ErrorDetail($"Can not find order {request.OrderId}"));

            if(order.OrderStatus != OrderStatus.Submitted)
                return AppResult.Invalid(new ErrorDetail(nameof(order.OrderStatus), $"Order must be {OrderStatus.Submitted.Name}"));

            var orderConfirmStockEvent = 
                new OrderConfirmStockIntegrationEvent(
                    order.Id,
                    order.ProductUnits
                );

            _ = _kafkaProducer.PublishAsync(orderConfirmStockEvent, ct).ConfigureAwait(false);

            return AppResult.Success("Successful");
        }
    }                                                                                           
}
