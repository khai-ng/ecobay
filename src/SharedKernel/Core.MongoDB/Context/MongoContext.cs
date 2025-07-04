using Core.MongoDB.OpenTelemetry;
using MongoDB.Driver;
using System.Reflection;

namespace Core.MongoDB.Context
{
    /// <summary>
    ///  Need no implement <see cref="IDisposable"/>, MongoClient handled it automaticly
    /// </summary>
    public class MongoContext
    {
        private readonly IMongoDatabase _database;
        private readonly List<Func<Task>> _commands = [];

        public MongoContext(MongoContextOptions dbSettings) 
        {
            var mongoUrl = new MongoUrl(dbSettings.ConnectionString);
            var clientSettings = MongoClientSettings.FromUrl(mongoUrl);
            if (dbSettings.Telemetry.Enable)
            {
                var options = new InstrumentationOptions { CaptureCommandText = true };
                clientSettings.ClusterConfigurator = cb => cb.Subscribe(new DiagnosticsActivityEventSubscriber(options));
            }

            _database = new MongoClient(clientSettings).GetDatabase(mongoUrl.DatabaseName);
        }

        public void AddCommand(Func<Task> func)
        {
            _commands.Add(func);
        }

        public async Task SaveChangesAsync(CancellationToken ct = default)
        {
            foreach (var command in _commands)
            {
                await command.Invoke();
            }
            _commands.Clear();
        }

        public IMongoCollection<T> Collection<T>()
        {
            var collectionAttribute = typeof(T).GetCustomAttribute<MongoCollectionAttribute>();
            var collection = collectionAttribute != null ? collectionAttribute.CollectionName : typeof(T).Name;
            return _database!.GetCollection<T>(collection);
        }
    }
}
