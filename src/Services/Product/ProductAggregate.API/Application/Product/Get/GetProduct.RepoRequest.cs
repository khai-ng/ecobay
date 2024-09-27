using Core.Pagination;
using ProductAggregate.API.Application.ConsistentHashing;

namespace ProductAggregate.API.Application.Product.Get
{
    public class GetProductRepoRequest : PagingRequest
    {
        public GetProductRepoRequest() { }
        public GetProductRepoRequest(
            AppHost channel,
            string category,
            int pageIndex,
            int pageSize) : base(pageIndex, pageSize)
        {
            Channel = channel;
            Category = category;
        }
        public AppHost Channel { get; set; }
        public string DbName { get; set; }
        public string Category { get; set; }
    }

}
