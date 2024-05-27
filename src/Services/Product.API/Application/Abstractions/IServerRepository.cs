using Core.Autofac;
using Core.MongoDB.Repository;
using Product.API.Domain.ServerAggregate;

namespace Product.API.Application.Abstractions
{
    public interface IServerRepository : IRepository<Server>, IScoped
    {
    }
}
