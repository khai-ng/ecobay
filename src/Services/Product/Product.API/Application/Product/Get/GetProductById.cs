//using Grpc.Core;
//using GrpcProduct.Get;
//using MongoDB.Bson;
//using Product.API.Application.Common.Abstractions;

//namespace Product.API.Application.Product.GetProduct
//{
//    public class GetProductById : GetProductService.GetProductServiceBase
//    {
//        private readonly IProductRepository _productRepository;
//        public GetProductById(IProductRepository productRepository)
//        {
//            _productRepository = productRepository;
//        }

//        public override async Task<GetProductByIdResponse> GetById(GetProductByIdRequest request,
//            ServerCallContext context)
//        {
//            var listId = request.Ids.Select(x => ObjectId.Parse(x));
//            _productRepository.SetCollection(request.VNode);

//            var collection = await _productRepository.GetAsync(listId);

//            var rs = new GetProductByIdResponse() { };

//            rs.Data.AddRange(collection.Select(item => new ProductItemResponse()
//            {
//                MainCategory = item.MainCategory,
//                Title = item.Title,
//                Price = item.Price ?? "",
//            }));
//            return rs;
//        }
//    }
//}
