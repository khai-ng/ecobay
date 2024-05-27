using Core.Autofac;
using Core.MongoDB.Paginations;
using Core.Result.AppResults;
using Core.Result.Paginations;
using Hangfire;
using MongoDB.Driver;
using Product.API.Application.Abstractions;
using Product.API.Domain.ServerAggregate;

namespace Product.API.Application.BackgroudJob
{
    public interface IProductMigrationJob
    {
        [AutomaticRetry(Attempts = 0)]
        Task<AppResult<string>> ProductMigrationJobAsync();
    }
    public class ProductMigrationJob : IProductMigrationJob, ITransient
    {
        private List<ProductServer> _productServer = [];

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

        public async Task<AppResult<string>> ProductMigrationJobAsync()
        {
            var iterationSize = 100_000;
            var batchSize = 2_000;

            _ = await CreateHashRingAsync();
            var products = await FluentPaging
                .From(new PagingRequest(1, iterationSize))
                .PagingAsync(_originProductRepository.Collection.Find(x => x.Version < 1));

            var tasks = new List<Task>();
            var batches = products.Data.Select((product, index) => new { Index = index, Product = product })
                        .GroupBy(x => x.Index / batchSize)
                        .Select(group => group.Select(x => x.Product).ToList())
                        .ToList();

            foreach (var batchData in batches)
            {
                tasks.Add(BatchHashingAsync(batchData));
            }
            await Task.WhenAll(tasks);

            var count = 0;
            foreach (var item in _productServer)
            {
                _productRepository.SetConnection($"mongodb://admin:1234@127.0.0.1:{item.Server.Port}");
                _productRepository.SetDatabase(item.Server.Database);
                _productRepository.SetCollection(item.Server.Collection);
                count += item.Products.Count;
                await _productRepository.Collection.InsertManyAsync(item.Products);
            }

            var filter = Builders<Domain.ProductAggregate.Product>.Filter.In(x => x.Id, products.Data.Select(x => x.Id));
            var update = Builders<Domain.ProductAggregate.Product>.Update.Set(x => x.Version, 1);
            await _originProductRepository.Collection.UpdateManyAsync(filter, update);

            return AppResult.Success<string>($"Done: {count}");
        }

        private async Task BatchHashingAsync(IEnumerable<Domain.ProductAggregate.Product> products)
        {
            foreach (var p in products)
            {
                var hashedServer = _hashRingManager.HashRing.GetBucket(p.Id.ToString());
                var productServer = _productServer.Single(x => hashedServer != null && hashedServer.Id.Equals(x.Server.Id));

                productServer.Products.Add(p);
            }
        }

        private async Task<bool> CreateHashRingAsync()
        {
            var servers = await _serverRepository.GetAllAsync();
            _productServer = servers.Select(s => new ProductServer(s)).ToList();
            if (!_hashRingManager.HashRing.IsInit)
            {
                _hashRingManager.HashRing.Init(servers);
            }
            return true;
            
        }

        internal class ProductServer
        {
            public Server Server { get; set; }
            public List<Domain.ProductAggregate.Product> Products { get; set; } = new();

            internal ProductServer(Server server) { Server = server; }
        }
    }
}
