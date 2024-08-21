using Amazon.Runtime.Internal.Transform;
using Core.Autofac;
using Core.Result.AppResults;
using Core.Result.Paginations;
using Grpc.Net.Client;
using GrpcProduct;
using MediatR;
using MongoDB.Driver;
using ProductAggregate.API.Application.Abstractions;
using ProductAggregate.API.Application.Dto;

namespace ProductAggregate.API.Application.Product
{
    public class GetProductHandler : IRequestHandler<GetProductRequest, AppResult<PagingResponse<GetProductResponse>>>, ITransient
    {
        private readonly Dictionary<string, Channel> _channelMap = new([
            new("product-db-1", new() { Host = "product-api-1", Port = "81" }),
            new("product-db-2", new() { Host = "product-api-2", Port = "81" }),
            new("product-db-3", new() { Host = "product-api-3", Port = "81" })
            ]);


        private readonly Serilog.ILogger _logger;
        private readonly IHashRingManager _hashRingManager;       
        public GetProductHandler(Serilog.ILogger logger, IHashRingManager hashRingManager)
        {
            _logger = logger;
            _hashRingManager = hashRingManager;
        }

        public async Task<AppResult<PagingResponse<GetProductResponse>>> Handle(
            GetProductRequest request,
            CancellationToken ct)
        {
            var req = new GrpcProduct.GetProductRequest()
            {
                Category = request.Category,
                PageInfo = new PagingInfo() 
                { 
                    PageIndex = request.PageIndex, 
                    PageSize = Convert.ToInt32(Math.Ceiling((decimal)request.PageSize/_channelMap.Count))
                }
            };
            var fluentPaging = FluentPaging.From(request);
            List<Task<GrpcProduct.GetProductResponse>> tasks = []; 

            foreach (var item in _channelMap)
            {
                tasks.Add(GetProductItemFronChannel(item.Value, req));
            }
            var responses = await Task.WhenAll(tasks);

            var rs = fluentPaging.Result(
                responses.Take(request.PageSize)
                .SelectMany(r => r.Data)
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

        private async Task<GrpcProduct.GetProductResponse> GetProductItemFronChannel(Channel service, GrpcProduct.GetProductRequest request)
        {
            using var channel = GrpcChannel.ForAddress($"http://{service.Host}:{service.Port}");
            var client = new GrpcProduct.Product.ProductClient(channel);
            var rs = await client.GetItemAsync(request);

            _logger
                .ForContext("Channel", service, true)
                .ForContext("Request", request, true)
                .Information($"Grpc has sended {nameof(GetProductRequest)}");

            return rs;
        }
    }
}
