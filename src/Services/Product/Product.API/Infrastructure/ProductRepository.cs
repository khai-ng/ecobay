namespace Product.API.Infrastructure
{
    public class ProductRepository : Repository<ProductItem>, IProductRepository, ITransient
    {
        private readonly AppDbContext _context;
        public ProductRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PagingResponse<ProductItem>> GetPagingAsync(GetProductRequest request)
        {
            var fluentPaging = FluentPaging.From(request);

            var masterData = _context.Products
                .Find(x => x.MainCategory.Equals(request.Category));

            var filterdData = await fluentPaging
                .FilterApply(masterData)
                .ToListAsync()
                .ConfigureAwait(false);
            return fluentPaging.Result(filterdData);
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