using Core.Autofac;
using Core.MongoDB.Paginations;
using Core.Result.AppResults;
using Core.Result.Paginations;
using MongoDB.Driver;
using Product.API.Application.Abstractions;
using Product.API.Domain.ServerAggregate;

namespace Product.API.Application.BackgroudJob
{
    public interface IProductMigrationJob
    {
        Task<AppResult<string>> ProductMigrationJobAsync();
    }
    public class ProductMigrationJob : IProductMigrationJob, ITransient
    {
        //private BTreeHashing<Server> _hashRing = new();
        private List<ProductServer> _productServer;

        private readonly IServerRepository _serverRepository;
        private readonly IProductRepository _productRepository;
        private readonly IHashRingManager _hashRingManager;
        public ProductMigrationJob(IServerRepository serverRepository,
            IProductRepository productRepository,
            IHashRingManager hashRingManager)
        {
            _serverRepository = serverRepository;
            _productRepository = productRepository;
            _hashRingManager = hashRingManager;
        }

        public async Task<AppResult<string>> ProductMigrationJobAsync()
        {                  
            _ = await CreateHashRingAsync();
            var products = await FluentPaging
                .From(new PagingRequest(1, 1000))
                .PagingAsync(_productRepository.Collection.Find(x => x.Version != 1));

            _productRepository.UpdateProductVersion(products.Data.Select(x => x.Id));

            var filter = Builders<Domain.ProductAggregate.Product>.Filter.In(x => x.Id, products.Data.Select(x => x.Id));
            var update = Builders<Domain.ProductAggregate.Product>.Update.Set(x => x.Version, 1);
            await _productRepository.Collection.UpdateManyAsync(filter, update);

            var options = new ParallelOptions()
            {
                MaxDegreeOfParallelism = 10
            };

            await Parallel.ForEachAsync(products.Data, async (p, ct) =>
            {
                var hashedServer = _hashRingManager.HashRing.GetBucket(p.Id.ToString());
                var productServer = _productServer
                    .SingleOrDefault(x => hashedServer != null && hashedServer.Id.Equals(x.Server.Id));

                if (productServer != null)
                    productServer.Products.Add(p);
            });

            foreach (var item in _productServer)
            {
                _productRepository.SetConnection($"mongodb://admin:1234@127.0.0.1:{item.Server.Port}");
                _productRepository.SetDatabase(item.Server.Database);
                _productRepository.SetCollection(item.Server.Collection);

                await _productRepository.Collection.InsertManyAsync(item.Products);
            }

            return AppResult.Success<string>("OK");
        }

        private async Task<bool> CreateHashRingAsync()
        {
            if(!_hashRingManager.HashRing.IsInit)
            {
                var servers = await _serverRepository.GetAllAsync();

                _productServer = servers.Select(s => new ProductServer(s)).ToList();

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
