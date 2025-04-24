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
            var result = await _productRepository.GetPagingAsync(
                request,
                x => new ProductItemDto
                {
                    Id = x.Id.ToString(),
                    MainCategory = x.MainCategory,
                    Title = x.Title,
                    AverageRating = x.AverageRating,
                    RatingNumber = x.RatingNumber,
                    Price = x.Price,
                    Image = x.Images?.Select(x => x.Large).FirstOrDefault(),
                    Store = x.Store
                }).ConfigureAwait(false);

            return AppResult.Success(result);
        }
    }
}
