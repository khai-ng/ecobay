namespace Product.API.Application.Abstractions
{

    public interface IProductRepository : IRepository<ProductItem, ObjectId>
    {
        Task<PagingResponse<TDestination>> GetPagingAsync<TDestination>(
            GetProductQuery request,
            Func<ProductItem, TDestination> selector)
            where TDestination : class;

        Task<IEnumerable<ProductItem>> GetByIdAsync(IEnumerable<ObjectId> ids);
    }
}