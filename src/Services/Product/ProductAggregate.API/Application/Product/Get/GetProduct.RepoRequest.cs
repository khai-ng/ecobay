using Core.Result.Paginations;
using ProductAggregate.API.Application.ConsistentHashing;

namespace ProductAggregate.API.Application.Product.Get
{
    public class GetProductRepoRequest : PagingRequest
    {
        public GetProductRepoRequest() { }
        public GetProductRepoRequest(
            ChannelDto channel,
            string category,
            int pageIndex,
            int pageSize) : base(pageIndex, pageSize)
        {
            Channel = channel;
            Category = category;
        }
        public ChannelDto Channel { get; set; }
        public string Category { get; set; }
    }

}
