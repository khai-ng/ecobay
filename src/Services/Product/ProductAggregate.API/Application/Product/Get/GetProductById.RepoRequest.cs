using ProductAggregate.API.Application.ConsistentHashing;

namespace ProductAggregate.API.Application.Product.Get
{
    public class GetProductByIdRepoRequest(string dbName, AppHost channel, IEnumerable<string> productIds)
    {
        public string DbName { get; set; } = dbName;
        public AppHost Channel { get; set; } = channel;
        public IEnumerable<string> ProductIds { get; set; } = productIds;
    }
}
