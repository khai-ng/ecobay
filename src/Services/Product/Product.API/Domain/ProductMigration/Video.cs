namespace Product.API.Domain.ProductMigration
{
    public class Video
    {
        public string title { get; set; }
        public string url { get; set; }
        //[JsonPropertyName("user_id")]
        public string user_id { get; set; }
    }
}
