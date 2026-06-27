namespace Product.API.Presentation.Endpoint
{
    public class GetProductRequest
    {
        public string? Category { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
