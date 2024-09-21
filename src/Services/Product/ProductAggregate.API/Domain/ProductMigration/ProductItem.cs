using Core.MongoDB.ServiceDefault;
using MongoDB.Bson.Serialization.Attributes;
using ProductAggregate.API.Application.ConsistentHashing;
using System.Text.Json.Serialization;

namespace ProductAggregate.API.Domain.ProductMigration
{
    [BsonIgnoreExtraElements]
    public class ProductItem : AggregateRoot
    {
        public string main_category { get; set; }
        public string title { get; set; }
        public decimal average_rating { get; set; }
        public decimal rating_number { get; set; }
        public IEnumerable<string>? features { get; set; }
        public IEnumerable<string>? description { get; set; }
        [JsonConverter(typeof(StringConverter))]
        public string? price { get; set; }
        public IEnumerable<Image>? images { get; set; }
        public IEnumerable<Video>? videos { get; set; }
        public string? store { get; set; }
        public IEnumerable<string>? categories { get; set; }
        public object? details { get; set; }

        public int? virtualId { get; set; }
    }
}
