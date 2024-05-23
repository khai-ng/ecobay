using Core.Autofac;
using Core.MongoDB.Paginations;
using Core.Result.AppResults;
using Core.Result.Paginations;
using MediatR;
using MongoDB.Driver;
using Product.API.Application.Abstractions;

namespace Product.API.Application
{
    public class GetProduct : IRequestHandler<GetProductRequest, AppResult<PagingResponse<GetProductResponse>>>, IScoped
    {
        private readonly IProductRepository _productRepository;

        public GetProduct(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<AppResult<PagingResponse<GetProductResponse>>> Handle(
            GetProductRequest request, 
            CancellationToken cancellationToken)
        {
            var fluentPaging = FluentPaging.From(request);
            var masterData = _productRepository.DbSet
                .Find(x => x.MainCategory.Equals(request.Category));

            var filterdData = await fluentPaging.Filter(masterData)
                .Project(x => new GetProductResponse()
                {
                    Id = x.Id.ToString(),
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
                }).ToListAsync();

            return fluentPaging.Result(filterdData);
        }
    }
}
