using Core.SharedKernel;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Threading;

namespace Core.MongoDB.Context
{
    public class MongoContext : IMongoContext, IUnitOfWork
    {
        private MongoClient _mongoClient;
        private IMongoDatabase _database;
        private string _collectionName;
        //private IClientSessionHandle Session;

        private readonly List<Func<Task>> _commands = [];
        private readonly MongoDbSetting _mongoDbSetting;

        public MongoContext(IOptions<MongoDbSetting> dbSettings) 
        {
            _mongoDbSetting = dbSettings.Value;
        }

        public void Dispose()
        {
            //Session?.Dispose();
            GC.SuppressFinalize(this);
        }

        public void AddCommand(Func<Task> func)
        {
            _commands.Add(func);
        }

        public Task SaveChangesAsync(CancellationToken ct = default)
        {
            if (_mongoClient == null)
            {
                SetConnection(_mongoDbSetting.ConnectionString);
                SetDatabase(_mongoDbSetting.DatabaseName);
            }

            //using (Session = await _mongoClient!.StartSessionAsync(cancellationToken: ct))
            //{
            //    Session.StartTransaction();

            //    await Session.CommitTransactionAsync(ct);
            //}

            var commandTasks = _commands.Select(c => c.Invoke());
            return Task.WhenAll(commandTasks);
        }

        public IMongoCollection<T> GetCollection<T>()
        {
            if(_database is null)
            {
                SetConnection(_mongoDbSetting.ConnectionString);
                SetDatabase(_mongoDbSetting.DatabaseName);
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
