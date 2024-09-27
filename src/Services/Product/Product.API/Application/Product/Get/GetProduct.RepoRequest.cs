using Core.Pagination;

namespace Product.API.Application.Product.GetProducts
{
    public class GetProductRepoRequest : PagingRequest
    {
        public string DbName { get; set; }
        public string Category { get; set; }
    }
}
