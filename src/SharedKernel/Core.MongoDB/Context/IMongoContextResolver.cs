namespace Core.MongoDB.Context
{
    public interface IMongoContextResolver
    {
        void SetConnection(string connectionString);
        void SetDatabase(string databaseName);
    }
}
