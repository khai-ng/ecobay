using ProductAggregate.API.Domain.ProductAggregate;

namespace ProductAggregate.API.Application.Product.Get
{
    public class GetProductRepoResponse(IEnumerable<ProductItem> productItems, bool hasNext)
    {
        public IEnumerable<ProductItem> ProductItems { get; set; } = productItems;
        public bool HasNext { get; set; } = hasNext;
    }

}
