using MongoDB.Bson;

namespace Product.API.Application.Product
{
    public class GetProductByIdRequest
    {
        public IEnumerable<ObjectId> Ids { get; set; }
    }
}
