using ProductAggregate.API.Application.ConsistentHashing;

namespace ProductAggregate.API.Application.Product.Get
{
    public class GetProductByIdRepoRequest(ChannelDto channel, string vNode, IEnumerable<string> productIds)
    {
        public ChannelDto Channel { get; set; } = channel;
        public string VNode { get; set; } = vNode;
        public IEnumerable<string> ProductIds { get; set; } = productIds;
    }
}
