using Amazon.Runtime.Internal.Transform;
using Core.Autofac;
using Core.MongoDB.Paginations;
using Core.Result.AppResults;
using Core.Result.Paginations;
using Grpc.Net.Client;
using GrpcProduct;
using MediatR;
using MongoDB.Driver;
using ProductAggregate.API.Application.Abstractions;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;

namespace ProductAggregate.API.Application.Product
{
    public class GetProductHandler : IRequestHandler<GetProductRequest, AppResult<PagingResponse<GetProductResponse>>>, IScoped
    {
        private readonly IProductRepository _productRepository;
        private readonly Serilog.ILogger _logger;
        private readonly Dictionary<string, Channel> _channelMap = [
            new("product-db-1", new() { Host = "product-api-1", Port = "81" }),
            new("product-db-2", new() { Host = "product-api-2", Port = "81" }),
            new("product-db-3", new() { Host = "product-api-3", Port = "81" }),
        ];
        public GetProductHandler(IProductRepository productRepository, Serilog.ILogger logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        public async Task<AppResult<PagingResponse<GetProductResponse>>> Handle(
            GetProductRequest request,
            CancellationToken cancellationToken)
        {
            var req = new ProductRequest()
            {
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Category = request.Category
            };
            var fluentPaging = FluentPaging.From(request);
            List<Task<PagingProductResponse>> tasks = [];

            foreach (var item in _channelMap)
            {
                tasks.Add(GetProductItemFronChannel(item.Value, req));
            }
            var responses = await Task.WhenAll(tasks);

            var rs = fluentPaging.Result(
                responses.SelectMany(r => r.Data)
                .Select(x => new GetProductResponse()
                {
                    MainCategory = x.MainCategory,
                    Title = x.Title,
                    Price = x.Price,
                })
            );
            rs.HasNext = responses.Any(x => x.HasNext);
            return rs;
        }

        private async Task<PagingProductResponse> GetProductItemFronChannel(Channel service, ProductRequest request)
        {
            using var channel = GrpcChannel.ForAddress($"http://{service.Host}:{service.Port}");
            var client = new GrpcProduct.Product.ProductClient(channel);
            var rs = await client.GetItemAsync(request);

            _logger
                .ForContext("Channel", service, true)
                .ForContext("Request", request, true)
                .ForContext("Response", rs.Data.FirstOrDefault(), true)
                .Information("Grpc has sended get product item request");

            return rs;
        }
    }
}
