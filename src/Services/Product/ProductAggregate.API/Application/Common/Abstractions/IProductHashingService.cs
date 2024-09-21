using Core.ConsistentHashing;
using ProductAggregate.API.Application.ConsistentHashing;
using ProductAggregate.API.Application.Hashing;
using ProductAggregate.API.Domain.ServerAggregate;

namespace ProductAggregate.API.Application.Common.Abstractions
{
    public interface IProductHashingService
    {
        Task<IDictionary<VirtualNode<Server>, List<T>>> HashProductAsync<T>(IEnumerable<T> product)
            where T : IHashable;

        AppHost? TryGetChannel(string host);
    }
}
