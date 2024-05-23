using Core.SharedKernel;
using MongoDB.Bson;

namespace Core.MongoDB.ServiceDefault
{

    public interface IEntity : IEntity<ObjectId> { }
}
