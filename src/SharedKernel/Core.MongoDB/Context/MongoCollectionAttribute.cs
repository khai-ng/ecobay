namespace Core.MongoDB.Context
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class MongoCollectionAttribute(string collectionName): Attribute
    {
        public string CollectionName { get; set; } = collectionName;
    }
}
