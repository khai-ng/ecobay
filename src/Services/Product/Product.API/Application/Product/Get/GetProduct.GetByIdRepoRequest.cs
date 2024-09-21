using MongoDB.Bson;

namespace Product.API.Application.Product.Get
{
    public record GetProductByIdRepoRequest(string DbName, IEnumerable<ObjectId> ProductIds);
}
