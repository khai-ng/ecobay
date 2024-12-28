using System.Linq;

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
                DbName = request.DbName,
                Category = request.Category,
                PageIndex = request.PageInfo.PageIndex,
                PageSize = request.PageInfo.PageSize
            };
            var response = await _productRepository.GetPagingAsync(repoRequest);
            if (response == null) return new GetProductResponse()
            {
                PageInfo = request.PageInfo,
                HasNext = false,
            };

            var rs = new GetProductResponse()
            {
                PageInfo = request.PageInfo,
                HasNext = response.HasNext,
                Data = { response.Data
                    .Select(item => new ProductItemResponse()
                    {
                        MainCategory = item.MainCategory,
                        Title = item.Title,
                        AverageRating = Convert.ToDouble(item.AverageRating),
                        RatingNumber = Convert.ToDouble(item.RatingNumber),
                        Price = item.Price ?? "",
                        Images = { item.Images?
                            .Select(x => new GrpcProduct.Get.Image()
                            {
                                Thumb = x.Thumb,
                                Large = x.Large,
                                Variant = x.Variant,
                                Hires = x.Hires ?? ""
                            })
                            ?? Array.Empty<GrpcProduct.Get.Image>()
                        },
                        Store = item.Store ?? "",
                        Categories = { item.Categories ?? Array.Empty<string>() }
                    }) 
                }
            };
            return rs;
        }

        public override async Task<GetProductByIdResponse> GetById(GetProductByIdRequest request,
            ServerCallContext context)
        {
            var listId = new GetProductByIdRepoRequest(
                request.DbName,
                request.Ids.Select(x => ObjectId.Parse(x))
                );

            var collection = await _productRepository.GetAsync(listId);

            var rs = new GetProductByIdResponse() 
            {
                Data = { collection
                    .Select(item => new ProductItemResponse()
                    {
                        MainCategory = item.MainCategory,
                        Title = item.Title,
                        AverageRating = Convert.ToDouble(item.AverageRating),
                        RatingNumber = Convert.ToDouble(item.RatingNumber),
                        Price = item.Price ?? "",
                        Images = { item.Images?
                            .Select(x => new GrpcProduct.Get.Image()
                            {
                                Thumb = x.Thumb,
                                Large = x.Large,
                                Variant = x.Variant,
                                Hires = x.Hires ?? ""
                            })
                            ?? Array.Empty<GrpcProduct.Get.Image>()
                        },
                        Store = item.Store ?? "",
                        Categories = { item.Categories ?? Array.Empty<string>() }
                    })
                }
            };
            return rs;
        }
    }
}
