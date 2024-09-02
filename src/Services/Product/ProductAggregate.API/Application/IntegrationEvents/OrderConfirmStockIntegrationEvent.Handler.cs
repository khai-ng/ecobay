using Amazon.Runtime.Internal;
using Core.Autofac;
using Core.IntegrationEvents.IntegrationEvents;
using Core.Kafka.Producers;
using Core.Result.AppResults;
using Grpc.Net.Client;
using MediatR;
using ProductAggregate.API.Application.Common.Abstractions;
using ProductAggregate.API.Application.ConsistentHashing;
using ProductAggregate.API.Application.Product.Update;
using ProductAggregate.API.Infrastructure.Shared;
using System.Threading.Channels;

namespace ProductAggregate.API.Application.IntegrationEvents
{
    
    public class OrderConfirmStockIntegrationEventHandler :
        IIntegrationEventHandler<OrderConfirmStockIntegrationEvent>, ITransient
    {
        private readonly IProductHashingService _productHashingService;
        private readonly IKafkaProducer _producer;
        private readonly Serilog.ILogger _logger;

        public OrderConfirmStockIntegrationEventHandler(IProductHashingService productHashingService, IKafkaProducer producer, Serilog.ILogger logger)
        {
            _productHashingService = productHashingService;
            _producer = producer;
            _logger = logger;
        }

        public async Task HandleAsync(OrderConfirmStockIntegrationEvent @event, CancellationToken ct = default)
        {
            //var request = new ConfimStockRequest(@event.ProductUnits);
            //await _mediator.Send(request, ct);

            var productUnitHashed = await _productHashingService.HashProductAsync(@event.ProductUnits);

            List<Task<GrpcProduct.Update.UpdateProductUnitResponse>> confirmStockTasks = [];
            foreach (var productUnit in productUnitHashed)
            {
                var server = productUnit.Key.Node;
                var rsChannel = GrpcServerMapping.GrpcServerMap.TryGetValue(server.Host, out var channel);
                if (!rsChannel)
                {
                    _logger.ForContext(typeof(OrderConfirmStockIntegrationEvent))
                        .ForContext("Channel", channel)
                        .Fatal("Can not found the matching resource server");

                    var publishEvent = new OrderConfirmStockFailedIntegrationEvent(@event.OrderId, "Order produt not found");
                    await _producer.PublishAsync(publishEvent, ct);
                    return;
                }

                var req = new GrpcProduct.Update.ConfirmStockRequest() { };
                req.ProductUnits.AddRange(
                    productUnit.Value
                    .Select(x => new GrpcProduct.Update.ProductUnit()
                    {
                        Id = x.Id,
                        Units = x.Units
                    }));

                confirmStockTasks.Add(ConfirmStockFromChannel(channel!, req));
            }

            var confirmStockResults = await Task.WhenAll(confirmStockTasks);
            if (confirmStockResults.Any(x => !x.IsSuccess))
            {
                //TODO: reverse change for each product service

                _logger.ForContext(typeof(OrderConfirmStockIntegrationEvent))
                        .Error("");

                var failedEvent = new OrderConfirmStockFailedIntegrationEvent(@event.OrderId, "Confirm stock order failed");
                await _producer.PublishAsync(failedEvent, ct);
                return;
            }

            var successEvent = new OrderConfirmStockSuccessIntegrationEvent(@event.OrderId);
            await _producer.PublishAsync(successEvent, ct);
            return;
        }

        private async Task<GrpcProduct.Update.UpdateProductUnitResponse> ConfirmStockFromChannel(
            ChannelModel service,
            GrpcProduct.Update.ConfirmStockRequest request)
        {
            using var channel = GrpcChannel.ForAddress($"http://{service.Host}:{service.Port}");
            var client = new GrpcProduct.Update.UpdateProductService.UpdateProductServiceClient(channel);
            var rs = await client.ConfirmStockAsync(request);

            _logger
                .ForContext("Channel", service, true)
                .ForContext("Request", request, true)
                .Information($"Grpc has sent {nameof(GrpcProduct.Update.ConfirmStockRequest)}");

            return rs;
        }
    }
}
