using Product.API.Application.Abstractions;

namespace Product.API.Application.Product.Get
{
    public class GetProductHandler : IRequestHandler<GetProductCommand, AppResult<PagingResponse<ProductItemDto>>>, ITransient
    {
        private readonly IProductRepository _productRepository;

        public GetProductHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<AppResult<PagingResponse<ProductItemDto>>> Handle(GetProductCommand query, CancellationToken cancellationToken)
        {
            var request = new GetProductRequest(query.Category, query.PageIndex, query.PageSize);
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
