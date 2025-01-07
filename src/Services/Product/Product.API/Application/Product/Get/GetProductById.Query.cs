namespace Product.API.Application.Product.Get
{
    public class GetProductByIdQuery: IRequest<AppResult<IEnumerable<ProductItemDto>>>
    {
        public IEnumerable<string> Ids { get; set; }
    }
}
