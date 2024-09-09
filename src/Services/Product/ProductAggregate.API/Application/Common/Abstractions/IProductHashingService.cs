using Core.ConsistentHashing;
using ProductAggregate.API.Application.ConsistentHashing;
using ProductAggregate.API.Application.Hashing;
using ProductAggregate.API.Domain.ServerAggregate;

namespace ProductAggregate.API.Application.Common.Abstractions
{
    public interface IProductHashingService
    {
        int ChannelUnits { get; }
        IEnumerable<ChannelDto> GetAllChannel();
        Task<IDictionary<VirtualNode<Server>, List<T>>> HashProductAsync<T>(IEnumerable<T> product)
            where T : IHashable;

        ChannelDto? TryGetChannelByDbName(string name);
    }
}
