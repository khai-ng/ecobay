using Core.Autofac;
using Core.Result.AppResults;
using Grpc.Net.Client;
using GrpcProduct.Get;
using MediatR;
using ProductAggregate.API.Application.Common.Abstractions;
using ProductAggregate.API.Application.ConsistentHashing;
using ProductAggregate.API.Infrastructure.Shared;

namespace ProductAggregate.API.Application.Product.GetProduct
{
    public class GetProductById : IRequestHandler<GetProductByIdRequest, AppResult<IEnumerable<GetProductByIdItemResponse>>>, ITransient
    {
        private readonly IProductHashingService _productHashingService;
        private readonly Serilog.ILogger _logger;
        public GetProductById(IProductHashingService productHashingService, Serilog.ILogger logger)
        {
            _productHashingService = productHashingService;
            _logger = logger;
        }

        public async Task<AppResult<IEnumerable<GetProductByIdItemResponse>>> Handle(GetProductByIdRequest request, CancellationToken ct)
        {
            var productHashingRequest = request.Ids.Select(x => new ProductHashingDto(x));
            var listProductHashed = await _productHashingService.HashProductAsync(productHashingRequest);

            List<Task<GetProductByIdResponse>> getProductTasks = [];
            foreach (var productHashed in listProductHashed)
            {
                var server = productHashed.Key.Node;
                var rsChannel = GrpcServerMapping.GrpcServerMap.TryGetValue(server.Host, out var channel);
                if (!rsChannel)
                    return AppResult.NotFound($"Can not found the matching resource server");

                var req = new GrpcProduct.Get.GetProductByIdRequest()
                {
                    VNode = server.Collection
                };
                req.Ids.AddRange(productHashed.Value.Select(x => x.Id));

                getProductTasks.Add(GetProductItemFromChannel(channel!, req));
            }

            var grpcGetProductResult = await Task.WhenAll(getProductTasks);

            var result = grpcGetProductResult
                .SelectMany(x => x.Data)
                .Select(x => new GetProductByIdItemResponse()
                {
                    MainCategory = x.MainCategory,
                    Title = x.Title,
                    Price = x.Price,
                });
            return AppResult.Success(result);

        }

        private async Task<GetProductByIdResponse> GetProductItemFromChannel(ChannelModel service, GrpcProduct.Get.GetProductByIdRequest request)
        {
#if DEBUG
            using var channel = GrpcChannel.ForAddress("http://localhost:1303");
#else
            using var channel = GrpcChannel.ForAddress($"http://{service.Host}:{service.Port}");
#endif
            var client = new GetProductService.GetProductServiceClient(channel);
            var rs = await client.GetByIdAsync(request);

            _logger
                .ForContext("Channel", service, true)
                .ForContext("Request", request, true)
                .Information($"Grpc has sent {nameof(GetProductByIdRequest)}");

            return rs;
        }
    }

    public class ProductHashingDto(string id) : IHashable
    {
        public string Id { get; set; } = id;
    }
}
