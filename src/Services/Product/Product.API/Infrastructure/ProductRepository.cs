using System.Linq;
using System.Linq.Expressions;

namespace Product.API.Infrastructure
{
    public class ProductRepository : Repository<ProductItem>, IProductRepository, ITransient
    {
        private readonly AppDbContext _context;
        public ProductRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PagingResponse<TDestination>> GetPagingAsync<TDestination>(
            GetProductRequest request, 
            Func<ProductItem, TDestination> selector) 
            where TDestination : class
        {
            var fluentPaging = FluentPaging.From(request);

            var masterData = _context.Products
                .Find(x => x.MainCategory.Equals(request.Category));

            var filterdData = await fluentPaging
                .FilterApply(masterData)
                .ToListAsync()
                .ConfigureAwait(false);

            return fluentPaging.Result(filterdData.Select(selector));
        }

        public async Task<IEnumerable<ProductItem>> GetByIdAsync(IEnumerable<ObjectId> ids)
        {
            return await _context.Products
                .Find(x => ids.Contains(x.Id))
                .ToListAsync()
                .ConfigureAwait(false);
        }
    }
}