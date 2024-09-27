using Core.Autofac;
using Core.IntegrationEvents.IntegrationEvents;
using Core.Kafka.Producers;
using Core.AppResults;
using ProductAggregate.API.Application.Common.Abstractions;
using ProductAggregate.API.Application.Product.Update;

namespace ProductAggregate.API.Application.IntegrationEvents
{

    public class OrderConfirmStockIntegrationEventHandler :
        IIntegrationEventHandler<OrderConfirmStockIntegrationEvent>, ITransient
    {
        private readonly IProductHashingService _productHashingService;
        private readonly IKafkaProducer _producer;
        private readonly Serilog.ILogger _logger;
        private readonly IProductRepository _productRepository;

        public OrderConfirmStockIntegrationEventHandler(
            IProductHashingService productHashingService,
            IKafkaProducer producer,
            Serilog.ILogger logger,
            IProductRepository productRepository)
        {
            _productHashingService = productHashingService;
            _producer = producer;
            _logger = logger;
            _productRepository = productRepository;
        }

        public async Task HandleAsync(OrderConfirmStockIntegrationEvent @event, CancellationToken ct = default)
        {
            var productUnitHashed = await _productHashingService.HashProductAsync(@event.ProductUnits);

            List<Task<AppResult>> confirmStockTasks = [];
            foreach (var productUnit in productUnitHashed)
            {
                var server = productUnit.Key.Node;
                var channel = _productHashingService.TryGetChannel(server.Host);
                if (channel == null)
                {
                    _logger.ForContext(typeof(OrderConfirmStockIntegrationEvent))
                        .Fatal("The matching resource server not found");

                    var publishEvent = new OrderConfirmStockFailedIntegrationEvent(@event.OrderId, "Order product not found");
                    await _producer.PublishAsync(publishEvent, ct);
                    return;
                }

                var repoRequest = new ConfimStockRepoRequest(server.Database, channel, productUnit.Value);
                //TODO: dispatch dbName
                confirmStockTasks.Add(_productRepository.ChannelConfimStockAsync(repoRequest));
            }

            var confirmStockResults = await Task.WhenAll(confirmStockTasks);
            if (confirmStockResults.Any(x => !x.IsSuccess))
            {
                var errMessage = confirmStockResults!
                    .Where(x => !x.IsSuccess)
                    .SelectMany(x => x.Errors)
                    .Select(x => x.Message)
                    .FirstOrDefault();

                //TODO: reverse change for each product service
                //
                _logger.ForContext(typeof(OrderConfirmStockIntegrationEvent))
                        .Error(errMessage ?? "");

                var failedEvent = new OrderConfirmStockFailedIntegrationEvent(@event.OrderId, "Confirm stock order failed");
                await _producer.PublishAsync(failedEvent, ct);
                return;
            }

            var successEvent = new OrderConfirmStockSuccessIntegrationEvent(@event.OrderId);
            await _producer.PublishAsync(successEvent, ct);
            return;
        }
    }
}
