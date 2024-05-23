using MongoDB.Driver;

namespace Core.MongoDB.Context
{
    public interface IMongoContext
    {
        void AddCommand(Func<Task> func);
        IMongoCollection<T> GetCollection<T>(string collectionName);
    }
}
