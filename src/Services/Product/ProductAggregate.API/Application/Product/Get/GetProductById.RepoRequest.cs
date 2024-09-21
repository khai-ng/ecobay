using ProductAggregate.API.Application.ConsistentHashing;

namespace ProductAggregate.API.Application.Product.Get
{
    public class GetProductByIdRepoRequest(AppHost channel, string dbName, IEnumerable<string> productIds)
    {
        public AppHost Channel { get; set; } = channel;
        public string DbName { get; set; } = dbName;
        public IEnumerable<string> ProductIds { get; set; } = productIds;
    }
}
