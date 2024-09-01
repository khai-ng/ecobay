using Core.Autofac;
using Core.IntegrationEvents.IntegrationEvents;
using Core.Result.AppResults;
using Core.Result.Paginations;
using Grpc.Net.Client;
using GrpcProduct.Update;
using MediatR;
using ProductAggregate.API.Application.ConsistentHashing;
using ProductAggregate.API.Application.IntegrationEvents;
using ProductAggregate.API.Infrastructure.Shared;
using static System.Net.Mime.MediaTypeNames;

namespace ProductAggregate.API.Application.Product.GetProduct
{
    public class GetProductHandler : IRequestHandler<GetProductRequest, AppResult<PagingResponse<GetProductItemResponse>>>, ITransient
    {
        private readonly Serilog.ILogger _logger;
        private readonly IServiceProvider _serviceProvider;
        public GetProductHandler(Serilog.ILogger logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public async Task<AppResult<PagingResponse<GetProductItemResponse>>> Handle(
            GetProductRequest request,
            CancellationToken ct)
        {
            Test();
            return AppResult.Success();

            var req = new GrpcProduct.Get.GetProductRequest()
            {
                Category = request.Category,
                PageInfo = new GrpcProduct.Get.PagingInfo()
                {
                    PageIndex = request.PageIndex,
                    PageSize = Convert.ToInt32(Math.Ceiling((decimal)request.PageSize / GrpcServerMapping.GrpcServerMap.Count))
                }
            };
            var fluentPaging = FluentPaging.From(request);
            List<Task<GrpcProduct.Get.GetProductResponse>> tasks = [];

            foreach (var item in GrpcServerMapping.GrpcServerMap)
            {
                tasks.Add(GetProductItemFromChannel(item.Value, req));
            }
            var responses = await Task.WhenAll(tasks);

            var rs = fluentPaging.Result(
                responses.Take(request.PageSize)
                .SelectMany(r => r.Data)
                .Select(x => new GetProductItemResponse()
                {
                    MainCategory = x.MainCategory,
                    Title = x.Title,
                    Price = x.Price,
                })
            );
            rs.HasNext = responses.Any(x => x.HasNext);
            return rs;
        }

        private async Task<GrpcProduct.Get.GetProductResponse> GetProductItemFromChannel(ChannelModel channel, GrpcProduct.Get.GetProductRequest request)
        {
            using var grpcChannel = GrpcChannel.ForAddress($"http://{channel.Host}:{channel.Port}");
            var client = new GrpcProduct.Get.GetProductService.GetProductServiceClient(grpcChannel);
            var rs = await client.GetItemAsync(request);

            _logger
                .ForContext("Channel", channel, true)
                .ForContext("Request", request, true)
                .Information($"Grpc has sent {nameof(GetProductRequest)}");

            return rs;
        }

        private void Test()
        {
            using var scope = _serviceProvider.CreateScope();
            //IntegrationEvent @event = new OrderConfirmStockIntegrationEvent(Guid.NewGuid(), new List<Dto.Product.ProductUnit>());
            //var handlerType = typeof(IIntegrationEventHandler<>).MakeGenericType(@event.GetType());
            //if (handlerType is null) return;

            var handlers = scope.ServiceProvider.GetService(typeof(IIntegrationEventHandler<>));

            //foreach (var handler in handlers)
            //{
            //    var methodInfo = eventHandleType.GetMethod(nameof(IIntegrationEventHandler<IntegrationEvent>.HandleAsync));
            //}
        }
    }
}
