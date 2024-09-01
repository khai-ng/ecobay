using Core.Autofac;
using Core.ConsistentHashing;
using ProductAggregate.API.Application.Common.Abstractions;
using ProductAggregate.API.Domain.ServerAggregate;

namespace ProductAggregate.API.Infrastructure.Shared
{
    public class ProductHashingService: IProductHashingService, ISingleton
    {
        private readonly IHashRingManager _hashRingManager;
        public ProductHashingService(IHashRingManager hashRingManager)
        {
            _hashRingManager = hashRingManager;
        }
        public async Task<IDictionary<VirtualNode<Server>, List<T>>> HashProductAsync<T>(IEnumerable<T> product)
            where T: IHashable
        {
            await _hashRingManager.TryInit();
            Dictionary<VirtualNode<Server>, List<T>> storage = [];
            List<Task> batches = [];

            foreach (var p in product)
            {
                batches.Add(Task.Run(() =>
                {
                    var hashedVNode = _hashRingManager.HashRing.GetBucket(p.Id);
                    var vNode = storage.Keys.SingleOrDefault(x => x == hashedVNode);
                    if (vNode is null)
                        storage.Add(hashedVNode, []);

                    storage[hashedVNode].Add(p);
                    return Task.CompletedTask;
                }));
            }
            await Task.WhenAll(batches);

            return storage;
        }
    }
}
