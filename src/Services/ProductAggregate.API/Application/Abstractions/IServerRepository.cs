using Core.Autofac;
using Core.MongoDB.Repository;
using ProductAggregate.API.Domain.ServerAggregate;

namespace ProductAggregate.API.Application.Abstractions
{
    public interface IServerRepository : IRepository<Server>, IScoped
    {
    }
}
