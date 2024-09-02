using Core.Autofac;
using Core.Kafka.Producers;
using Core.Result.AppResults;
using Grpc.Net.Client;
using MediatR;
using ProductAggregate.API.Application.Common.Abstractions;
using ProductAggregate.API.Application.ConsistentHashing;
using ProductAggregate.API.Infrastructure.Shared;

namespace ProductAggregate.API.Application.Product.Update
{
    [Obsolete("Moved to confirm stock integration event")]
    public class ConfimStockHandler : IRequestHandler<ConfimStockRequest, AppResult>, ITransient
    {
        private readonly IProductHashingService _productHashingService;
        private readonly Serilog.ILogger _logger;
        private readonly IKafkaProducer _producer;

        public ConfimStockHandler(IProductHashingService productHashingService, Serilog.ILogger logger, IKafkaProducer producer)
        {
            _productHashingService = productHashingService;
            _logger = logger;
            _producer = producer;
        }

        public async Task<AppResult> Handle(ConfimStockRequest request, CancellationToken cancellationToken)
        {
            var hashingRequest = request.ProductUnits;
            var productUnitHashed = await _productHashingService.HashProductAsync(hashingRequest);

            List<Task<GrpcProduct.Update.UpdateProductUnitResponse>> getProductTasks = [];
            foreach (var productUnit in productUnitHashed)
            {
                var server = productUnit.Key.Node;
                var rsChannel = GrpcServerMapping.GrpcServerMap.TryGetValue(server.Host, out var channel);
                if (!rsChannel)
                    return AppResult.NotFound($"Can not found the matching resource server");

                var req = new GrpcProduct.Update.ConfirmStockRequest() {};
                req.ProductUnits.AddRange(
                    productUnit.Value
                    .Select(x => new GrpcProduct.Update.ProductUnit()
                    {
                        Id = x.Id,
                        Units = x.Units
                    }));

                getProductTasks.Add(GetProductItemFromChannel(channel!, req));
            }

            var grpcGetProductResult = await Task.WhenAll(getProductTasks);
            if(grpcGetProductResult.Any(x => !x.IsSuccess))
            {
                //TODO: reverse change for each product service
                //
                return AppResult.Error(grpcGetProductResult.Select(x => x.Message).ToArray());
            }
            return AppResult.Success();
        }

        private async Task<GrpcProduct.Update.UpdateProductUnitResponse> GetProductItemFromChannel(
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
