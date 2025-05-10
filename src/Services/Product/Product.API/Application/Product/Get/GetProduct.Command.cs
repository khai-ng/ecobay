namespace Product.API.Application.Product.Get
{
    public record GetProductCommand(string? Category, int PageIndex, int PageSize) : PagingRequest(PageIndex, PageSize), IRequest<AppResult<PagingResponse<ProductItemDto>>>
    { }
}
