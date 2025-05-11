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

                var convertProductQty = @event.ProductQty
                    .Select(x => new
                    {
                        Id = ObjectId.Parse(x.Id),
                        x.Qty
                    });

                var products = await _productRepository.GetByIdAsync(convertProductQty.Select(x => x.Id)).ConfigureAwait(false);
                if (convertProductQty.Count() != products.Count())
                {
                    var publishEvent = new OrderConfirmStockFailedIntegrationEvent(@event.OrderId, "Order product not found");
                    await _producer.PublishAsync(publishEvent, ct).ConfigureAwait(false);
                    return;
                }

                foreach (var item in products)
                {
                    item.Qty -= convertProductQty.Single(x => x.Id == item.Id).Qty;
                }

                _productRepository.UpdateRange(products);
                await _unitOfWork.SaveChangesAsync(ct).ConfigureAwait(false);

                var successEvent = new OrderConfirmStockSuccessIntegrationEvent(@event.OrderId);
                await _producer.PublishAsync(successEvent, ct).ConfigureAwait(false);
                return;
            }
            catch (Exception ex)
            {
                var publishEvent = new OrderConfirmStockFailedIntegrationEvent(@event.OrderId, ex.Message);
                await _producer.PublishAsync(publishEvent, ct).ConfigureAwait(false);
            }           
        }
    }
}
