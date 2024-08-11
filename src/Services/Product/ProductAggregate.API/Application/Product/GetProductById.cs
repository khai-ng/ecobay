using Core.Autofac;
using Core.Result.AppResults;
using Grpc.Net.Client;
using MediatR;
using ProductAggregate.API.Application.Abstractions;
using ProductAggregate.API.Application.Dto;
using GrpcProduct;

namespace ProductAggregate.API.Application.Product
{
    public class GetProductById : IRequestHandler<GetProductByIdRequest, AppResult<IEnumerable<ProductItemResponse>>>, ITransient
    {

        private readonly Dictionary<string, Channel> _channelMaps = new([
            new("product-db-1", new() { Host = "product-api-1", Port = "81" }),
            new("product-db-2", new() { Host = "product-api-2", Port = "81" }),
            new("product-db-3", new() { Host = "product-api-3", Port = "81" })
            ]);

        private readonly IHashRingManager _hashRingManager;
        private readonly Serilog.ILogger _logger;
        public GetProductById(IHashRingManager hashRingManager, Serilog.ILogger logger)
        {
            _hashRingManager = hashRingManager;
            _logger = logger;
        }

        public async Task<AppResult<IEnumerable<ProductItemResponse>>> Handle(GetProductByIdRequest request, CancellationToken ct)
        {
            await _hashRingManager.Init();
            var hashedVNode = _hashRingManager.HashRing.GetBucket(request.Ids.First());
            var rsChannel = _channelMaps.TryGetValue(hashedVNode.Node.Host, out var channel);
            if (!rsChannel)
                return AppResult.Error($"Error get service: {hashedVNode.Node.Database}");

            var req = new GetProducByIdRequest()
            {
                VNode = hashedVNode.Node.Collection
            };
            req.Ids.AddRange(request.Ids.Select(x => x.ToString()));

            var rs = await GetProductItemFronChannel(channel, req);
            return AppResult.Success(rs.Data.ToList().AsEnumerable());

        }

        private async Task<GetProducByIdResponse> GetProductItemFronChannel(Channel service, GetProducByIdRequest request)
        {
            using var channel = GrpcChannel.ForAddress($"http://{service.Host}:{service.Port}");
            var client = new GrpcProduct.Product.ProductClient(channel);
            var rs = await client.GetByIdAsync(request);

            _logger
                .ForContext("Channel", service, true)
                .ForContext("Request", request, true)
                .ForContext("Response", rs.Data.FirstOrDefault(), true)
                .Information("Grpc has sended GetByIdAsync");

            return rs;
        }
    }
}
