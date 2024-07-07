using Core.Autofac;
using Core.MongoDB.Context;
using Core.MongoDB.Repository;
using MongoDB.Driver;
using Product.API.Application.Abstractions;
using Product.API.Domain.ProductAggregate;
using Core.MongoDB.Paginations;
using Core.Result.Paginations;
using Product.API.Application.Grpc;

namespace Product.API.Infrastructure
{
    public class ProductRespository : Repository<ProductItem>, IProductRepository, ITransient
    {
        private readonly IMongoContext _context;
        public ProductRespository(IMongoContext context) : base(context)
        {
            _context = context;
            SetCollection("vnode1");
        }

        public async Task<PagingResponse<ProductItem>> GetAsync(GetProductRequest request)
        {
            var fluentPaging = FluentPaging.From(request);

            var masterData = Collection
                .Find(x => x.MainCategory.Equals(request.Category));

            var filterdData = await fluentPaging.Filter(masterData).ToListAsync();
            return fluentPaging.Result(filterdData);
        }

        public async Task<IEnumerable<ProductItem>> GetByIdAsync(GetProductByIdRequest request)
        {
            return await Collection.Find(x => request.Ids.Contains(x.Id)).ToListAsync();
        }
    }
}