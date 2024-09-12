using Core.Autofac;
using Core.Result.AppResults;
using Grpc.Net.Client;
using ProductAggregate.API.Application.Common.Abstractions;
using ProductAggregate.API.Application.ConsistentHashing;
using ProductAggregate.API.Application.Product.Get;
using ProductAggregate.API.Application.Product.Update;
using ProductAggregate.API.Domain.ProductAggregate;

namespace ProductAggregate.API.Infrastructure
{
    public class ProductRepository: IProductRepository, ITransient
    {
        private readonly Serilog.ILogger _logger;

        public ProductRepository(Serilog.ILogger logger)
        {
            _logger = logger;
        }

        public async Task<AppResult<GetProductRepoResponse>> GetAsync(GetProductRepoRequest request)
        {
            try
            {
                var grpcRequest = new GrpcProduct.Get.GetProductRequest()
                {
                    Category = request.Category,
                    PageInfo = new GrpcProduct.Get.PagingInfo()
                    {
                        PageIndex = request.PageIndex,
                        PageSize = request.PageSize
                    }
                };

                using var channel = GetGrpcChannel(request.Channel);
                var client = new GrpcProduct.Get.GetProductService.GetProductServiceClient(channel);
                var grpcResponse = await client.GetItemAsync(grpcRequest);

                _logger
                    .ForContext("Channel", request.Channel, true)
                    .ForContext("Request", grpcRequest, true)
                    .Information($"Grpc has sent {nameof(GrpcProduct.Get.GetProductRequest)}");

                var products = grpcResponse.Data.Select(x => new ProductItem()
                {
                    MainCategory = x.MainCategory,
                    Title = x.Title,
                    Price = x.Price,
                });

                var rs = new GetProductRepoResponse(products, grpcResponse.HasNext);

                return AppResult.Success(rs);
            }
            catch (Exception ex)
            {
                return AppResult.Error(ex.Message);
            }
        }

        public async Task<AppResult<IEnumerable<ProductItem>>> GetByIdAsync(GetProductByIdRepoRequest request)
        {
            try
            {
                var grpcRequest = new GrpcProduct.Get.GetProductByIdRequest()
                {
                    VNode = request.VNode
                };
                grpcRequest.Ids.AddRange(request.ProductIds);

                using var channel = GetGrpcChannel(request.Channel);
                var client = new GrpcProduct.Get.GetProductService.GetProductServiceClient(channel);
                var grpcResponse = await client.GetByIdAsync(grpcRequest);

                _logger
                    .ForContext("Channel", request.Channel, true)
                    .ForContext("Request", grpcRequest, true)
                    .Information($"Grpc has sent {nameof(GrpcProduct.Get.GetProductByIdRequest)}");

                var products = grpcResponse.Data
                    .Select(x => new ProductItem()
                    {
                        MainCategory = x.MainCategory,
                        Title = x.Title,
                        Price = x.Price,
                    });

                return AppResult.Success(products);
            }
            catch (Exception ex)
            {
                return AppResult.Error(ex.Message);
            }

        }

        public async Task<AppResult> ChannelConfimStockAsync(ConfimStockRepoRequest request)
        {
            try
            {
                var grpcRequest = new GrpcProduct.Update.ConfirmStockRequest() { };
                grpcRequest.ProductUnits.AddRange(
                    request.ProductUnits
                    .Select(x => new GrpcProduct.Update.ProductUnit()
                    {
                        Id = x.Id,
                        Units = x.Units
                    }));

                using var channel = GetGrpcChannel(request.Channel);
                var client = new GrpcProduct.Update.UpdateProductService.UpdateProductServiceClient(channel);
                var grpcResult = await client.ConfirmStockAsync(grpcRequest);
                _logger
                    .ForContext("Channel", request.Channel, true)
                    .ForContext("Request", grpcRequest, true)
                    .Information($"Grpc has sent {nameof(GrpcProduct.Update.ConfirmStockRequest)}");

                if (grpcResult.IsSuccess)
                    return AppResult.Success();

                return AppResult.Error(grpcResult.Message);
            }
            catch (Exception ex)
            {
                return AppResult.Error(ex.Message);
            }           
        }

        private static GrpcChannel GetGrpcChannel(ChannelDto channel)
            => GrpcChannel.ForAddress($"http://{channel.Host}:{channel.Port}");
    }
}
