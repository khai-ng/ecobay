using Core.MongoDB.OpenTelemetry;
using MongoDB.Driver;
using Serilog;
using System.Reflection;

namespace Core.MongoDB.Context
{
    public class MongoContext
    {
        private MongoClient _mongoClient;
        private IMongoDatabase _database;

        private readonly List<Func<Task>> _commands = [];
        private readonly MongoContextOptions _dbSetting;

        private readonly ILogger _logger;

        public MongoContext(ILogger logger) 
        {
            _logger = logger;
        }

        public MongoContext(MongoContextOptions dbSettings) 
        {
            _dbSetting = dbSettings;
            SetDatabase(_dbSetting.Connection.DatabaseName);
        }

        public void AddCommand(Func<Task> func)
        {
            _commands.Add(func);
        }

        public Task SaveChangesAsync(CancellationToken ct = default)
        {
            if (_database is null)
                SetDatabase(_dbSetting.Connection.DatabaseName);           

            var commandTasks = _commands.Select(c => c.Invoke());
            return Task.WhenAll(commandTasks);
        }

        public MongoContext SetConnection(string connectionString)
        {
            var clientSettings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
            var options = new InstrumentationOptions { CaptureCommandText = true };
            if (_dbSetting.Telemetry.Enable)         
                clientSettings.ClusterConfigurator = cb => cb.Subscribe(new DiagnosticsActivityEventSubscriber(options));

            _mongoClient = new MongoClient(clientSettings);

            return this;
        }

        public MongoContext SetDatabase(string databaseName)
        {
            if (_mongoClient is null)
                SetConnection(_dbSetting.Connection.ConnectionString);

            _database = _mongoClient!.GetDatabase(databaseName);

            return this;
        }

        public IMongoCollection<T> Collection<T>()
        {
            if(_database is null)
                SetDatabase(_dbSetting.Connection.DatabaseName);

            var dbNameAttr = typeof(T).GetCustomAttribute<MongoDbNameAttribute>();
            var collection = dbNameAttr != null ? dbNameAttr.DbName : typeof(T).Name;
            return _database!.GetCollection<T>(collection);
        }
    }
}
