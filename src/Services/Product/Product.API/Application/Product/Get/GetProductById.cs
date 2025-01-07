namespace Product.API.Application.Product.Get
{
    public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, AppResult<IEnumerable<ProductItemDto>>>, ITransient
    {
        private readonly IProductRepository _productRepository;

        public GetProductByIdHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<AppResult<IEnumerable<ProductItemDto>>> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
        {

            var products = await _productRepository.GetByIdAsync(query.Ids.Select(x => ObjectId.Parse(x)));
            var result = products
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
            return AppResult.Success(result);
        }
    }
}
