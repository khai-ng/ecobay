using Core.Autofac;
using Core.MongoDB.Context;
using Core.MongoDB.Repository;
using Product.API.Application.Abstractions;
using Product.API.Domain.ServerAggregate;

namespace Product.API.Infrastructure
{
    public class ServerRepository : Repository<Server>, IServerRepository, ITransient
    {
        public ServerRepository(IMongoContext context) : base(context)
        {
        }
    }
}
