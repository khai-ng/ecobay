using Product.API.Application.Abstractions;

namespace Product.API.Application.Product.Get
{
    public class GetProductByIdHandler : IRequestHandler<GetProductByIdCommand, AppResult<IEnumerable<ProductItemDto>>>, ITransient
    {
        private readonly IProductRepository _productRepository;

        public GetProductByIdHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<AppResult<IEnumerable<ProductItemDto>>> Handle(GetProductByIdCommand query, CancellationToken cancellationToken)
        {

            var products = await _productRepository.GetByIdAsync(query.Ids.Select(x => ObjectId.Parse(x))).ConfigureAwait(false);
            var result = products
                .Select(x => new ProductItemDto
                {
                    MainCategory = x.MainCategory,
                    Title = x.Title,
                    AverageRating = x.AverageRating,
                    RatingNumber = x.RatingNumber,
                    Price = x.Price,
                    Store = x.Store,
                });
            return AppResult.Success(result);
        }
    }
}
