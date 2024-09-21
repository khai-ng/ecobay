namespace Core.MongoDB.Context
{
    public class MongoDbOptions
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;
    }
}
