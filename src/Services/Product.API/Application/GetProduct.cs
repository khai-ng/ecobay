using Core.Autofac;
using Core.MongoDB.Context;
using Core.MongoDB.Paginations;
using Core.Result.AppResults;
using Core.Result.Paginations;
using MediatR;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Product.API.Application.Abstractions;

namespace Product.API.Application
{
    public class GetProduct : IRequestHandler<GetProductRequest, AppResult<PagingResponse<Domain.Product>>>, IScoped
    {
        private readonly IProductRepository _productRepository;

        public GetProduct(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<AppResult<PagingResponse<Domain.Product>>> Handle(
            GetProductRequest request, 
            CancellationToken cancellationToken)
        {
            var data = _productRepository.DbSet
                .Find(x => x.MainCategory.Equals(request.Category));

            return await FluentPaging.From(request).PagingAsync(data);
        }
    }
}
