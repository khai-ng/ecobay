using Core.SharedKernel;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Core.MongoDB.Context
{
    public class MongoContext : IMongoContext, IUnitOfWork
    {
        private MongoClient _mongoClient;
        private IMongoDatabase _database;
        private string _collectionName;

        private readonly List<Func<Task>> _commands = [];
        private readonly IOptions<MongoDbSetting> _dbSettings;

        private IClientSessionHandle Session;
        public MongoContext(IOptions<MongoDbSetting> dbSettings) 
        {
            _dbSettings = dbSettings;
        }

        public void Dispose()
        {
            Session?.Dispose();
            GC.SuppressFinalize(this);
        }

        public void AddCommand(Func<Task> func)
        {
            _commands.Add(func);
        }

        public async Task SaveChangesAsync(CancellationToken ct = default)
        {
            if (_mongoClient == null)
            {
                SetConnection(_dbSettings.Value.ConnectionString);
                SetDatabase(_dbSettings.Value.DatabaseName);
            }

            using (Session = await _mongoClient!.StartSessionAsync(cancellationToken: ct))
            {
                Session.StartTransaction();
                var commandTasks = _commands.Select(c => c.Invoke());
                await Task.WhenAll(commandTasks);
                await Session.CommitTransactionAsync(ct);
            }
        }

        public IMongoCollection<T> GetCollection<T>()
        {
            if(_database is null)
            {
                SetConnection(_dbSettings.Value.ConnectionString);
                SetDatabase(_dbSettings.Value.DatabaseName);
            }
            var collection = string.IsNullOrEmpty(_collectionName) ? typeof(T).Name : _collectionName;
            return _database!.GetCollection<T>(collection);
        }

        public void SetConnection(string connectionString)
        {
            _mongoClient = new MongoClient(connectionString);
        }

        public void SetDatabase(string databaseName)
        {
            ArgumentNullException.ThrowIfNull(_mongoClient, nameof(MongoClient));
            _database = _mongoClient.GetDatabase(databaseName);
        }

        public void SetCollection(string collectionName)
        {
            _collectionName = collectionName;
        }
    }
}
