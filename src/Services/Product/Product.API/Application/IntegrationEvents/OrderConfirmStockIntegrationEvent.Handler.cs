using Core.Entities;

namespace Product.API.Application.IntegrationEvents
{

    public class OrderConfirmStockIntegrationEventHandler :
        IIntegrationEventHandler<OrderConfirmStockIntegrationEvent>, ITransient
    {
        private readonly IKafkaProducer _producer;
        private readonly Serilog.ILogger _logger;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public OrderConfirmStockIntegrationEventHandler(
            IKafkaProducer producer,
            Serilog.ILogger logger,
            IProductRepository productRepository,
            IUnitOfWork unitOfWork)
        {
            _producer = producer;
            _logger = logger;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task HandleAsync(OrderConfirmStockIntegrationEvent @event, CancellationToken ct = default)
        {
            try
            {
                List<Task<AppResult>> confirmStockTasks = [];

                var cvtProductUnits = @event.ProductUnits
                    .Select(x => new
                    {
                        Id = ObjectId.Parse(x.Id),
                        x.Units
                    });

                var products = await _productRepository.GetByIdAsync(cvtProductUnits.Select(x => x.Id));
                if (cvtProductUnits.Count() != products.Count())
                {
                    var publishEvent = new OrderConfirmStockFailedIntegrationEvent(@event.OrderId, "Order product not found");
                    await _producer.PublishAsync(publishEvent, ct);
                    return;
                }

                foreach (var item in products)
                {
                    item.Unit -= cvtProductUnits.Single(x => x.Id == item.Id).Units;
                }

                _productRepository.UpdateRange(products);
                await _unitOfWork.SaveChangesAsync(ct);

                var successEvent = new OrderConfirmStockSuccessIntegrationEvent(@event.OrderId);
                await _producer.PublishAsync(successEvent, ct);
                return;
            }
            catch (Exception ex)
            {
                var publishEvent = new OrderConfirmStockFailedIntegrationEvent(@event.OrderId, ex.Message);
                await _producer.PublishAsync(publishEvent, ct);
            }           
        }
    }
}
