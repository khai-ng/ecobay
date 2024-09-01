using Core.Autofac;
using Core.ConsistentHashing;
using ProductAggregate.API.Domain.ServerAggregate;

namespace ProductAggregate.API.Infrastructure.Shared
{
    public interface IHashRingManager
    {
        BTreeHashing<Server> HashRing { get; }
        Task TryInit();
    }

    public class HashRingManager : IHashRingManager, ISingleton
    {
        private readonly IServiceProvider _serviceProvider;

        public BTreeHashing<Server> HashRing { get; private set; }

        public HashRingManager(IServiceProvider serviceProvider)
        {
            HashRing = new();
            _serviceProvider = serviceProvider;
        }

        public async Task TryInit()
        {
            if (HashRing.IsInit) return;

            using var scope = _serviceProvider.CreateScope();
            var serverRepository = scope.ServiceProvider.GetRequiredService<IServerRepository>();
            var servers = await serverRepository.GetAllAsync();

            HashRing.Init(servers);
        }
    }
}
