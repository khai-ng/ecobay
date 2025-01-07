namespace Product.API.Domain.ProductMigration
{
    public class Image
    {
        public string thumb { get; set; }
        public string large { get; set; }
        public string variant { get; set; }
        //[JsonPropertyName("hi_res")]
        public string? hi_res { get; set; }
    }
}
