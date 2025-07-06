namespace Product.API.Application.IntegrationEvents
{

    public class OrderConfirmStockIntegrationEventHandler :
        IIntegrationEventHandler<OrderConfirmStockIntegrationEvent>, ITransient
    {
        private readonly IIntegrationProducer _producer;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public OrderConfirmStockIntegrationEventHandler(
            IIntegrationProducer producer,
            IProductRepository productRepository,
            IUnitOfWork unitOfWork)
        {
            _producer = producer;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task HandleAsync(OrderConfirmStockIntegrationEvent @event, CancellationToken ct = default)
        {
            try
            {
                List<Task<AppResult>> confirmStockTasks = [];

                var cvtProductQty = @event.ProductQty
                    .Select(x => new
                    {
                        Id = ObjectId.Parse(x.Id),
                        x.Qty
                    });

                var products = await _productRepository.GetByIdAsync(cvtProductQty.Select(x => x.Id)).ConfigureAwait(false);
                if (cvtProductQty.Count() != products.Count())               
                    throw new Exception("Order product not found");
                
                foreach (var item in products)
                {
                    var eventProductQty = cvtProductQty.Single(x => x.Id == item.Id).Qty;
                    if (item.Qty < eventProductQty)
                        throw new Exception($"Product out of stock");

                    item.Qty -= eventProductQty;
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
