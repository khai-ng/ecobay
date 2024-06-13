using Core.Autofac;
using Core.MongoDB.Context;
using Core.MongoDB.Repository;
using ProductAggregate.API.Application.Abstractions;
using ProductAggregate.API.Domain.ServerAggregate;

namespace ProductAggregate.API.Infrastructure
{
    public class ServerRepository : Repository<Server>, IServerRepository, ITransient
    {
        public ServerRepository(IMongoContext context) : base(context)
        {
        }
    }
}
