namespace ProductAggregate.API.Application.ConsistentHashing
{
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
