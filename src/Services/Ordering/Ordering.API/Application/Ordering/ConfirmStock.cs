using Core.Events.EventStore;

namespace Ordering.API.Application.Services
{
    public class ConfirmStock : IRequestHandler<ConfirmStockRequest, AppResult<string>>, ITransient
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IKafkaProducer _kafkaProducer;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEventStoreRepository<Order> _eventStoreRepository;

        public ConfirmStock(
            IOrderRepository orderRepository,
            IKafkaProducer kafkaProducer,
            IUnitOfWork unitOfWork,
            IEventStoreRepository<Order> eventStoreRepository)
        {
            _orderRepository = orderRepository;
            _kafkaProducer = kafkaProducer;
            _unitOfWork = unitOfWork;
            _eventStoreRepository = eventStoreRepository;
        }

        public async Task<AppResult<string>> Handle(ConfirmStockRequest request, CancellationToken ct)
        {
            var order = await _orderRepository.GetByIdAsync(request.OrderId).ConfigureAwait(false);

            if (order == null)
                return AppResult.Invalid(new ErrorDetail($"Can not find order {request.OrderId}"));

            if(order.OrderStatus != OrderStatus.Submitted)
                return AppResult.Invalid(new ErrorDetail(nameof(order.OrderStatus), $"Order must be {OrderStatus.Submitted.Name}"));

            var orderConfirmStockEvent = 
                new OrderConfirmStockIntegrationEvent(
                    order.Id,
                    order.OrderItems.Select(x => new ProductUnit(x.ProductId, x.Unit))
                );

            _ = _kafkaProducer.PublishAsync(orderConfirmStockEvent, ct);

            return AppResult.Success("Successful");
        }
    }                                                                                           
}
