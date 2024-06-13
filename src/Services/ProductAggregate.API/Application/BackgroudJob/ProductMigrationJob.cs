using Core.Autofac;
using Core.ConsistentHashing;
using Core.MongoDB.Paginations;
using Core.Result.AppResults;
using Core.Result.Paginations;
using Hangfire;
using MongoDB.Bson;
using MongoDB.Driver;
using ProductAggregate.API.Application.Abstractions;
using ProductAggregate.API.Domain.ProductAggregate;
using ProductAggregate.API.Domain.ServerAggregate;

namespace ProductAggregate.API.Application.BackgroudJob
{
    public interface IProductMigrationJob
    {
        [AutomaticRetry(Attempts = 0)]
        Task<AppResult<string>> ProductMigrationJobAsync();
        [AutomaticRetry(Attempts = 0)]
        Task<AppResult<string>> UpdateVirtualAsync();
    }
    public class ProductMigrationJob : IProductMigrationJob, ITransient
    {
        private IEnumerable<Server> _servers = [];

        private readonly IServerRepository _serverRepository;
        private readonly IProductRepository _productRepository;
        private readonly IProductRepository _originProductRepository;
        private readonly IHashRingManager _hashRingManager;
        public ProductMigrationJob(IServerRepository serverRepository,
            IProductRepository productRepository,
            IProductRepository originProductRepository,
            IHashRingManager hashRingManager)
        {
            _serverRepository = serverRepository;
            _productRepository = productRepository;
            _originProductRepository = originProductRepository;
            _hashRingManager = hashRingManager;
        }

        private async Task BatchHashingProductIdAsync(IDictionary<VirtualNode<Server>, List<ObjectId>> storage,
            IEnumerable<ProductItem> products)
        {
            foreach (var p in products)
            {
                var hashedVNode = _hashRingManager.HashRing.GetBucket(p.Id.ToString());
                var vNode = storage.Keys.SingleOrDefault(x => x == hashedVNode);
                if (vNode is null)
                    storage.Add(hashedVNode, []);

                storage[hashedVNode].Add(p.Id);
            }
        }

        private async Task BatchHashingProductItemAsync(IDictionary<Server, List<ProductItem>> storage, 
            IEnumerable<ProductItem> products)
        {
            foreach (var p in products)
            {
                var hashedVNode = _hashRingManager.HashRing.GetBucket(p.Id.ToString());
                var vNode = storage.Keys.SingleOrDefault(x => x == hashedVNode.Node);
                if (vNode is null)
                    storage.Add(hashedVNode.Node, []);

                storage[hashedVNode.Node].Add(p);
            }
        }

        private async Task<bool> CreateHashRingAsync()
        {
            _servers = await _serverRepository.GetAllAsync();
            if (!_hashRingManager.HashRing.IsInit)
            {
                _hashRingManager.HashRing.Init(_servers);
            }
            return true;
        }

        private void SetConnection(Server server)
        {

#if DEBUG
            _productRepository.SetConnection($"mongodb://admin:1234@127.0.0.1:{server.Port}");
#else
            _productRepository.SetConnection($"mongodb://admin:1234@{server.Host}:{server.Port}");      
#endif

            _productRepository.SetDatabase(server.Database);
            _productRepository.SetCollection(server.Collection);
        }

        private async Task UpdateProductVirtualIdAsync(IEnumerable<ObjectId> productIds, int virtualId)
        {
            var filter = Builders<ProductItem>.Filter.In(x => x.Id, productIds);
            var update = Builders<ProductItem>.Update.Set(x => x.VirtualId, virtualId);
            await _productRepository.Collection.UpdateManyAsync(filter, update);
        }

        public async Task<AppResult<string>> ProductMigrationJobAsync()
        {
            var iterationSize = 200_000;
            var batchSize = 2_000;

            _ = await CreateHashRingAsync();
            var products = await FluentPaging
                .From(new PagingRequest(1, iterationSize))
                .PagingAsync(_originProductRepository.Collection.Find(x => x.Version != 1));

            if(products.Data == null || !products.Data.Any())
                return AppResult.Success<string>($"Done: 0");
  
            var batches = products.Data.Select((product, index) => new { Index = index, Product = product })
                .GroupBy(x => x.Index / batchSize)
                .Select(group => group.Select(x => x.Product).ToList())
                .ToList();

            var tasks = new List<Task>();
            Dictionary<Server, List<ProductItem>> storage = [];

            foreach (var batchData in batches)
            {
                tasks.Add(BatchHashingProductItemAsync(storage, batchData));
            }
            await Task.WhenAll(tasks);

            var count = 0;
            foreach (var item in storage)
            {
                SetConnection(item.Key);
                count += item.Value.Count;

                try
                {
                    //var filterDel = Builders<Product>.Filter
                    //    .In(x => x.Id, item.Products.Select(x => x.Id));
                    //await _productRepository.Collection.DeleteManyAsync(filterDel);

                    //await _productRepository.Collection.InsertManyAsync(item.Products);
                }
                catch (Exception)
                {
                    throw;
                }
            }

            //var filter = Builders<Product>.Filter.In(x => x.Id, products.Data.Select(x => x.Id));
            //var update = Builders<Product>.Update.Set(x => x.Version, 1);
            //await _originProductRepository.Collection.UpdateManyAsync(filter, update);

            return AppResult.Success<string>($"Done: {count}");
        }

        public async Task<AppResult<string>> UpdateVirtualAsync()
        {
            _ = await CreateHashRingAsync();

            var count = 0;
            foreach (var server in _servers)
            {
                SetConnection(server);

                var fields = Builders<ProductItem>.Projection.Include(p => p.Id);
                var products = await FluentPaging
                    .From(new PagingRequest(1, 200_000))
                    .PagingAsync(_productRepository.Collection
                        .Find(x => x.VirtualId == null)
                        .Project<ProductItem>(fields));

                if(products == null || !products.Data.Any())
                    continue;

                var batches = products.Data.Select((product, index) => new { Index = index, Product = product })
                .GroupBy(x => x.Index / 5000)
                .Select(group => group.Select(x => x.Product).ToList())
                .ToList();

                List<Task> tasks = [];
                Dictionary<VirtualNode<Server>, List<ObjectId>> storage = [];

                foreach (var batch in batches)
                {
                    tasks.Add(BatchHashingProductIdAsync(storage, batch));
                }
                await Task.WhenAll(tasks);

                foreach (var item in storage)
                {
                    await UpdateProductVirtualIdAsync(item.Value, item.Key.VirtualId);
                }
                count += products.Data.Count();
            }
            return AppResult.Success<string>($"Done: {count}");
        }


    }
}
