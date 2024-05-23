using MongoDB.Bson.Serialization.Attributes;

namespace Product.API.Domain.ProductAggregate
{
    public class Image
    {
        public string Thumb { get; set; }
        public string Large { get; set; }
        public string Variant { get; set; }
        [BsonElement("hi_res")]
        public string? Hires { get; set; }
    }
}
