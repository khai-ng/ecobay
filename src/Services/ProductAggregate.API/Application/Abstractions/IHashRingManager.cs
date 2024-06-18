using Core.ConsistentHashing;
using ProductAggregate.API.Domain.ServerAggregate;

namespace ProductAggregate.API.Application.Abstractions
{
    public interface IHashRingManager
    {
        BTreeHashing<Server> HashRing { get; }
        Task Init();
    }
}
