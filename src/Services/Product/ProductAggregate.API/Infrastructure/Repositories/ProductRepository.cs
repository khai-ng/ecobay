using ProductAggregate.API.Domain.ProductAggregate;

namespace ProductAggregate.API.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository, ITransient
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
                    DbName = request.DbName,
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

                var products = grpcResponse.Data
                    .Select(x => new ProductItem()
                    {
                        MainCategory = x.MainCategory,
                        Title = x.Title,
                        AverageRating = Convert.ToDecimal(x.AverageRating),
                        RatingNumber = Convert.ToDecimal(x.RatingNumber),
                        Price = x.Price,
                        Images = x.Images.Select(i => new Image() 
                        {
                            Thumb = i.Thumb,
                            Large = i.Large,
                            Variant = i.Variant,
                            Hires = i.Hires
                        }),
                        Store = x.Store,
                        Categories = x.Categories
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
                    DbName = request.DbName
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
                        AverageRating = Convert.ToDecimal(x.AverageRating),
                        RatingNumber = Convert.ToDecimal(x.RatingNumber),
                        Price = x.Price,
                        Images = x.Images.Select(i => new Image()
                        {
                            Thumb = i.Thumb,
                            Large = i.Large,
                            Variant = i.Variant,
                            Hires = i.Hires
                        }),
                        Store = x.Store,
                        Categories = x.Categories
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
                var grpcRequest = new GrpcProduct.Update.ConfirmStockRequest() 
                {
                    DbName = request.DbName
                };
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

        private static GrpcChannel GetGrpcChannel(AppHost channel)
            => GrpcChannel.ForAddress($"http://{channel.Host}:{channel.Port}");
    }
}
