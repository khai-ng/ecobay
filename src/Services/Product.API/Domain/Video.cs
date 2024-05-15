using MongoDB.Bson.Serialization.Attributes;

namespace Product.API.Domain
{
    public class Video
    {
        public string Title { get; set; }
        public string Url { get; set; }
        [BsonElement("user_id")]
        public string UserId { get; set; }
    }
}
