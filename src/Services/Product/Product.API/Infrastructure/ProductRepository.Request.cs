namespace Product.API.Infrastructure
{
    public record GetProductRequest(
        string? Category, 
        int PageIndex, 
        int PageSize, 
        bool GetAll = false) : PagingRequest(PageIndex, PageSize, GetAll)
    { }
}
