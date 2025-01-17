namespace Product.API.Application.Product.Get
{
    public class GetProductHandler : IRequestHandler<GetProductQuery, AppResult<PagingResponse<ProductItemDto>>>, ITransient
    {
        private readonly IProductRepository _productRepository;

        public GetProductHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<AppResult<PagingResponse<ProductItemDto>>> Handle(GetProductQuery query, CancellationToken cancellationToken)
        {
            var request = new GetProductRequest
            {
                Category = query.Category,
                PageIndex = query.PageIndex,
                PageSize = query.PageSize,
            };
            var products = await _productRepository.GetPagingAsync(request).ConfigureAwait(false);
            var convertedData = products.Data
                .Select(x => new ProductItemDto
                {
                    MainCategory = x.MainCategory,
                    Title = x.Title,
                    AverageRating = x.AverageRating,
                    RatingNumber = x.RatingNumber,
                    Price = x.Price,
                    Images = x.Images,
                    Videos = x.Videos,
                    Store = x.Store,
                    Categories = x.Categories,
                    Details = x.Details,
                });

            var paging = FluentPaging.From(query);
            var result = paging.Result(convertedData);

            return AppResult.Success(result);
        }
    }
}
