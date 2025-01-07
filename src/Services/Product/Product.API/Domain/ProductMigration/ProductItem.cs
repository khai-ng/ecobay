using System.Text.Json;

namespace Product.API.Domain.ProductMigration
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

    public class StringConverter : JsonConverter<string>
    {
        public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var ok = reader.TryGetDecimal(out var dec);
            if (!ok)
                return reader.GetString();
            return dec.ToString();
        }

        public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
        => writer.WriteStringValue(value);
    }
}
