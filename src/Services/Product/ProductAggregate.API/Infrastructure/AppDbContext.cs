namespace ProductAggregate.API.Infrastructure
{
    public class AppDbContext : MongoContext
    {
        public AppDbContext(MongoContextOptions options) : base(options)
        {
        }

    }
}
