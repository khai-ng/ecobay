namespace ProductAggregate.API.Domain.ProductAggregate
{
    public class BsonPriceConverter : SerializerBase<string>
    {
        public override string Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var bsonType = context.Reader.GetCurrentBsonType();
            if(bsonType == MongoDB.Bson.BsonType.Null)
            {
                return "";
            }
            if (bsonType == MongoDB.Bson.BsonType.Double)
            {
                var doubleValue = context.Reader.ReadDouble();
                return doubleValue.ToString();
            }
            else if (bsonType == MongoDB.Bson.BsonType.String)
            {
                return context.Reader.ReadString();
            }
            else
            {
                return "";
            }
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, string value)
        {
            context.Writer.WriteString(value);
        }
    }
}
