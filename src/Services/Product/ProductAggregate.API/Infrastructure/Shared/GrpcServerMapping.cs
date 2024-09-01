using ProductAggregate.API.Application.ConsistentHashing;

namespace ProductAggregate.API.Infrastructure.Shared
{
    public static class GrpcServerMapping
    {

        public static readonly Dictionary<string, ChannelModel> GrpcServerMap = new([
            new("product-db-1", new() { Host = "product-api-1", Port = "81" }),
            new("product-db-2", new() { Host = "product-api-2", Port = "81" }),
            new("product-db-3", new() { Host = "product-api-3", Port = "81" })
            ]);
    }
}
