namespace Product.API.Domain.ProductAggregate
{
    public class BsonPriceConverter : SerializerBase<string>
    {
        public override string Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var bsonType = context.Reader.GetCurrentBsonType();
            switch (bsonType)
            {
                case BsonType.Null:
                    return "";
                case BsonType.Double:
                    var doubleValue = context.Reader.ReadDouble();
                    return doubleValue.ToString();
                case BsonType.String:
                    return context.Reader.ReadString();
                default:
                    return "";
            }
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, string value)
        {
            context.Writer.WriteString(value);
        }
    }
}
