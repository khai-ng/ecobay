using ProductAggregate.API.Application.ConsistentHashing;

namespace ProductAggregate.API.Application.Product.Update
{

    public class ConfimStockRepoRequest(AppHost channel, IEnumerable<ProductUnitDto> productUnit)
    {
        public AppHost Channel { get; set; } = channel;
        public IEnumerable<ProductUnitDto> ProductUnits { get; set; } = productUnit;
    }
}