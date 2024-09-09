using Grpc.Core;
using GrpcProduct.Get;
using MongoDB.Bson;
using Product.API.Application.Common.Abstractions;
using Product.API.Application.Product.GetProducts;

namespace Product.API.Application.Product.Get
{
    public class GetProduct : GetProductService.GetProductServiceBase
    {
        private readonly IProductRepository _productRepository;
        public GetProduct(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public override async Task<GetProductResponse> GetItem(
            GetProductRequest request,
            ServerCallContext context)
        {
            var repoRequest = new GetProductRepoRequest()
            {
                Category = request.Category,
                PageIndex = request.PageInfo.PageIndex,
                PageSize = request.PageInfo.PageSize
            };
            var response = await _productRepository.GetPagingAsync(repoRequest);

            var rs = new GetProductResponse()
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

        public override async Task<GetProductByIdResponse> GetById(GetProductByIdRequest request,
            ServerCallContext context)
        {
            var listId = request.Ids.Select(x => ObjectId.Parse(x));
            _productRepository.SetCollection(request.VNode);

            var collection = await _productRepository.GetAsync(listId);

            var rs = new GetProductByIdResponse() { };

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
