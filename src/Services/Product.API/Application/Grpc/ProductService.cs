using Grpc.Core;
using GrpcProduct;
using MongoDB.Bson;
using MongoDB.Driver;
using Product.API.Application.Abstractions;

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
        public override async Task<GrpcProduct.GetProductResponse> GetItem(
            GrpcProduct.GetProductRequest request, 
            ServerCallContext context)
        {
            var req = new GetProductRequest()
            {
                Category = request.Category,
                PageIndex = request.PageInfo.PageIndex,
                PageSize = request.PageInfo.PageSize
            };
            var response = await _productRepository.GetAsync(req);

            var rs = new GrpcProduct.GetProductResponse()
            {
                PageInfo = new PagingInfo() { PageIndex = response.PageIndex, PageSize = response.PageSize },
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

        public override async Task<GetProducByIdResponse> GetById(GetProducByIdRequest request, 
            ServerCallContext context)
        {
            var listId = request.Ids.Select(x => ObjectId.Parse(x));
            _productRepository.SetCollection(request.VNode);
            var collection = await _productRepository.Collection
                .Find(x => listId.Contains(x.Id))
                .ToListAsync();

            var rs = new GetProducByIdResponse()
            { };

            rs.Data.AddRange(collection.Select(item => new ProductItemResponse()
            {
                MainCategory = item.MainCategory,
                Title = item.Title,
                Price = item.Price ?? "",
            }));
            return rs;
        }
    }
}
