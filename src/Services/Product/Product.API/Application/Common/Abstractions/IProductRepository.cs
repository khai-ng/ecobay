namespace Product.API.Application.Common.Abstractions
{

    public interface IProductRepository: IRepository<ProductItem, ObjectId>
    {
        Task<PagingResponse<TDestination>> GetPagingAsync<TDestination>(
            GetProductRequest request, 
            Func<ProductItem, TDestination> selector) 
            where TDestination : class;

        Task<IEnumerable<ProductItem>> GetByIdAsync(IEnumerable<ObjectId> ids);
    }
}