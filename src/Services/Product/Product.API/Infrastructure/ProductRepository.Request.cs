namespace Product.API.Infrastructure
{
    public record GetProductRequest(
        string? Category, 
        int PageIndex, 
        int PageSize) : PagingRequest(PageIndex, PageSize)
    { }
}
