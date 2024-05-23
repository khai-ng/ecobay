namespace Core.MongoDB.Context
{
    public class MongoDbSetting
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;
    }
}
