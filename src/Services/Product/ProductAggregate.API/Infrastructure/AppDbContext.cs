using Core.Autofac;
using Core.MongoDB.Context;
using Microsoft.Extensions.Options;

namespace ProductAggregate.API.Infrastructure
{
    public class AppDbContext : MongoContext
    {
        public AppDbContext(IOptions<MongoDbOptions> dbSettings) : base(dbSettings.Value)
        {
        }

    }
}
