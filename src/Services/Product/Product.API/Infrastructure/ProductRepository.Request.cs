namespace Product.API.Infrastructure
{
    public class GetProductRequest : PagingRequest
    {
        public string Category { get; set; }
    }
}
