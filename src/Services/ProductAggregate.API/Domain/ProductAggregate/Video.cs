using MongoDB.Bson.Serialization.Attributes;

namespace ProductAggregate.API.Domain.ProductAggregate
{
    public class Video
    {
        public string Title { get; set; }
        public string Url { get; set; }
        [BsonElement("user_id")]
        public string UserId { get; set; }
    }
}
