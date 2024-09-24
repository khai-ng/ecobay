namespace Core.MongoDB.Context
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class MongoDbNameAttribute(string dbName): Attribute
    {
        public string DbName { get; set; } = dbName;
    }
}
