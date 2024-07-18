using Core.Result.Paginations;

namespace Product.API.Application.Product
{
    public class GetProductRequest : PagingRequest
    {
        public string Category { get; set; }
    }
}
