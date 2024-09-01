using Core.ConsistentHashing;
using ProductAggregate.API.Domain.ServerAggregate;
using ProductAggregate.API.Infrastructure.Shared;

namespace ProductAggregate.API.Application.Common.Abstractions
{
    public interface IProductHashingService
    {
        Task<IDictionary<VirtualNode<Server>, List<T>>> HashProductAsync<T>(IEnumerable<T> product)
            where T : IHashable;
    }
}
