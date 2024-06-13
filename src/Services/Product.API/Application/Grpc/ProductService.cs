using Grpc.Core;
using GrpcProduct;
using Product.API.Application.Abstractions;
using Product.API.Application.Product;

namespace Product.API.Application.Grpc
{
    public class ProductService: GrpcProduct.Product.ProductBase
    {
        private readonly IProductRepository _productRepository;
        private readonly Serilog.ILogger _logger;
        public ProductService(IProductRepository productRepository, Serilog.ILogger logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }
        public override async Task<PagingProductResponse> GetItem(ProductRequest request, ServerCallContext context)
        {
            var req = new GetProductRequest()
            {
                Category = request.Category,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };
            var response = await _productRepository.GetAsync(req);

            var rs = new PagingProductResponse()
            {
                PageIndex = response.PageIndex,
                PageSize = response.PageSize,
                HasNext = response.HasNext
            };

            rs.Data.AddRange(response.Data.Select(item => new ProductItemResponse()
            {
                MainCategory = item.MainCategory,
                Title = item.Title,
                Price = item.Price ?? "",
            }));
            return rs;
        }
    }
}
