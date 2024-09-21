using Core.Autofac;
using Core.MongoDB.Paginations;
using Core.MongoDB.Repository;
using Core.Repository;
using Core.Result.Paginations;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using Product.API.Application.Common.Abstractions;
using Product.API.Application.Product.Get;
using Product.API.Application.Product.GetProducts;
using Product.API.Domain.ProductAggregate;

namespace Product.API.Infrastructure
{

    public class ProductRespository : Repository<ProductItem>, IProductRepository, ITransient
    {
        private readonly AppDbContext _context;
        public ProductRespository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PagingResponse<ProductItem>> GetPagingAsync(GetProductRepoRequest request)
        {
            var fluentPaging = FluentPaging.From(request);

            _context.SetDatabase(request.DbName);
            var masterData = _context.ProductItems
                .Find(x => x.MainCategory.Equals(request.Category));

            var filterdData = await fluentPaging.Filter(masterData).ToListAsync();
            return fluentPaging.Result(filterdData);
        }

        public async Task<IEnumerable<ProductItem>> GetAsync(GetProductByIdRepoRequest request)
        {
            _context.SetDatabase(request.DbName);
            return await _context.ProductItems
                .Find(x => request.ProductIds.Contains(x.Id))
                .ToListAsync();
        }
    }
}