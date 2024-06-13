using Core.Autofac;
using Core.ConsistentHashing;
using ProductAggregate.API.Domain.ServerAggregate;

namespace ProductAggregate.API.Application.BackgroudJob
{
    public interface IHashRingManager
    {
        BTreeHashing<Server> HashRing { get; }
    }
    public class HashRingManager : IHashRingManager, ISingleton
    {
        public HashRingManager()
        {
            HashRing = new();
        }

        public BTreeHashing<Server> HashRing { get; private set; }

        public void Init(IEnumerable<Server> servers) => HashRing.Init(servers);
    }
}
