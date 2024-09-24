using MongoDB.Driver;
using System.Reflection;

namespace Core.MongoDB.Context
{
    public class MongoContext
    {
        private MongoClient _mongoClient;
        private IMongoDatabase _database;

        private readonly List<Func<Task>> _commands = [];
        private readonly MongoDbOptions _mongoDbSetting;

        public MongoContext() { }

        public MongoContext(MongoDbOptions dbSettings) 
        {
            _mongoDbSetting = dbSettings;
        }

        public void AddCommand(Func<Task> func)
        {
            _commands.Add(func);
        }

        public Task SaveChangesAsync(CancellationToken ct = default)
        {
            if (_database is null)
                SetDatabase(_mongoDbSetting.DatabaseName);           

            var commandTasks = _commands.Select(c => c.Invoke());
            return Task.WhenAll(commandTasks);
        }

        public void SetConnection(string connectionString)
        {
            _mongoClient = new MongoClient(connectionString);
        }

        public void SetDatabase(string databaseName)
        {
            if (_mongoClient is null)
                SetConnection(_mongoDbSetting.ConnectionString);

            _database = _mongoClient!.GetDatabase(databaseName);
        }

        public IMongoCollection<T> Collection<T>()
        {
            if(_database is null)
                SetDatabase(_mongoDbSetting.DatabaseName);

            var dbNameAttr = typeof(T).GetCustomAttribute<MongoDbNameAttribute>();
            var collection = dbNameAttr != null ? dbNameAttr.DbName : typeof(T).Name;
            return _database!.GetCollection<T>(collection);
        }
    }
}
