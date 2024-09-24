using ProductAggregate.API.Application.ConsistentHashing;

namespace ProductAggregate.API.Application.Product.Update
{

    public class ConfimStockRepoRequest(string dbName, AppHost channel, IEnumerable<ProductUnitDto> productUnit)
    {
        public string DbName { get; set; } = dbName;
        public AppHost Channel { get; set; } = channel;
        public IEnumerable<ProductUnitDto> ProductUnits { get; set; } = productUnit;
    }
}