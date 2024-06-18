using Core.Autofac;
using Core.ConsistentHashing;
using ProductAggregate.API.Application.Abstractions;
using ProductAggregate.API.Domain.ServerAggregate;

namespace ProductAggregate.API.Application.BackgroudJob
{
    public class HashRingManager : IHashRingManager, ISingleton
    {
        private readonly IServerRepository _serverRepository;
        public HashRingManager(IServerRepository serverRepository)
        {
            HashRing = new();
            _serverRepository = serverRepository;
        }

        public BTreeHashing<Server> HashRing { get; private set; }

        public async Task Init()
        {
            var servers = await _serverRepository.GetAllAsync();
            HashRing.Init(servers);
        }
    }
}
