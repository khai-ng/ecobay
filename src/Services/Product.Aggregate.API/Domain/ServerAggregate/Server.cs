using Core.MongoDB.ServiceDefault;
using MongoDB.Bson.Serialization.Attributes;
using System.IO.Hashing;
using System.Text;

namespace Product.API.Domain.ServerAggregate
{
    [BsonIgnoreExtraElements]
    public class Server: AggregateRoot
    {
        public string Host { get; set; }
        public string Port { get; set; }
        public string Database { get; set; }
        public string Collection {  get; set; }

        public override int GetHashCode()
        {
            var hash = XxHash32.HashToUInt32(Encoding.ASCII.GetBytes(Id.ToString()));
            return (int)hash;
        }
    }
}
