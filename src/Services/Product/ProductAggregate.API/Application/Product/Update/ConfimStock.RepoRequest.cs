using ProductAggregate.API.Application.ConsistentHashing;

namespace ProductAggregate.API.Application.Product.Update
{

    public class ConfimStockRepoRequest(ChannelDto channel, IEnumerable<ProductUnitDto> productUnit)
    {
        public ChannelDto Channel { get; set; } = channel;
        public IEnumerable<ProductUnitDto> ProductUnits { get; set; } = productUnit;
    }
}