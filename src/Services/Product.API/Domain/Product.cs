using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Product.API.Domain
{
    [BsonIgnoreExtraElements]
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        [BsonElement("main_category")]
        public string MainCategory { get; set; }
        public string Title { get; set; }
        [BsonElement("average_rating")]
        public decimal AverageRating {  get; set; }
        [BsonElement("rating_number")]
        public decimal RatingNumber {  get; set; }
        public IEnumerable<string>? Features { get; set; }
        public IEnumerable<string>? Description { get;set; }
        public decimal? Price { get; set; }
        public IEnumerable<Image>? Images { get; set; }
        public IEnumerable<Video>? Videos { get; set; }
        public string? Store { get; set; }
        public IEnumerable<string>? Categories { get; set; }
        public object? Details { get; set; }

    }
}
