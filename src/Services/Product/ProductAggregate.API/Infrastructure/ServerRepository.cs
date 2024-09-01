using Core.Autofac;
using Core.MongoDB.Context;
using Core.MongoDB.Repository;
using ProductAggregate.API.Domain.ServerAggregate;

namespace ProductAggregate.API.Infrastructure
{
    public interface IServerRepository : IRepository<Server>
    {
    }

    public class ServerRepository : Repository<Server>, IServerRepository, ITransient
    {
        public ServerRepository(IMongoContext context) : base(context)
        {
        }
    }
}
