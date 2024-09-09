using Core.Autofac;
using Core.ConsistentHashing;
using ProductAggregate.API.Application.Common.Abstractions;
using ProductAggregate.API.Application.ConsistentHashing;
using ProductAggregate.API.Domain.ServerAggregate;

namespace ProductAggregate.API.Application.Hashing
{
    public class ProductHashingService : IProductHashingService, ITransient
    {
        private readonly IHashRingManager _hashRingManager;
        private readonly Serilog.ILogger _logger;

        private static readonly Dictionary<string, ChannelDto> GrpcServerMap = new([
            new("product-db-1", new() { Host = "product-api-1", Port = "81" }),
            new("product-db-2", new() { Host = "product-api-2", Port = "81" }),
            new("product-db-3", new() { Host = "product-api-3", Port = "81" })
            ]);

        public ProductHashingService(IHashRingManager hashRingManager, Serilog.ILogger logger)
        {
            _hashRingManager = hashRingManager;
            _logger = logger;
        }

        public int ChannelUnits => GrpcServerMap.Count;
        public IEnumerable<ChannelDto> GetAllChannel() => GrpcServerMap.Values;

        public ChannelDto? TryGetChannelByDbName(string name)
        {
            var success = GrpcServerMap.TryGetValue(name, out var channel);
            if (!success) return null;
            return channel;
        }

        public async Task<IDictionary<VirtualNode<Server>, List<T>>> HashProductAsync<T>(IEnumerable<T> product)
            where T : IHashable
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
