namespace Product.API.Application.Common.Abstractions
{

    public interface IProductRepository: ICommandRepository<ProductItem, ObjectId>
    {
        Task<PagingResponse<ProductItem>> GetPagingAsync(GetProductRequest request);
        Task<IEnumerable<ProductItem>> GetByIdAsync(IEnumerable<ObjectId> ids);
    }
}