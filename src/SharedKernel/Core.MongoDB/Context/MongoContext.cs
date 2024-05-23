using Core.SharedKernel;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Core.MongoDB.Context
{
    public class MongoContext : IMongoContext, IUnitOfWork
    {
        private readonly MongoClient _mongoClient;
        private readonly List<Func<Task>> _commands = [];
        private readonly IMongoDatabase _database;
        private readonly IOptions<MongoDbSetting> _dbSettings;

        private IClientSessionHandle Session;
        public MongoContext(IOptions<MongoDbSetting> dbSettings) 
        {
            _dbSettings = dbSettings;
            _mongoClient = new MongoClient(_dbSettings.Value.ConnectionString);
            _database = _mongoClient.GetDatabase(_dbSettings.Value.DatabaseName);
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

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            using (Session = await _mongoClient.StartSessionAsync(cancellationToken: cancellationToken))
            {
                Session.StartTransaction();
                var commandTasks = _commands.Select(c => c.Invoke());
                await Task.WhenAll(commandTasks);
                await Session.CommitTransactionAsync(cancellationToken);
            }
        }

        public IMongoCollection<T> GetCollection<T>(string collectionName)
            => _database.GetCollection<T>(collectionName);
    }
}
