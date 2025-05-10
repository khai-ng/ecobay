namespace Product.API.Application.Product.Get
{
    public record GetProductByIdCommand(IEnumerable<string> Ids) : IRequest<AppResult<IEnumerable<ProductItemDto>>>
    { }
}
