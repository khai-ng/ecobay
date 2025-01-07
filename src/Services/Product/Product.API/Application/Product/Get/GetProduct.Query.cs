namespace Product.API.Application.Product.Get
{
    public class GetProductQuery: PagingRequest, IRequest<AppResult<PagingResponse<ProductItemDto>>>
    {
        public string Category { get; set; }
    }
}
