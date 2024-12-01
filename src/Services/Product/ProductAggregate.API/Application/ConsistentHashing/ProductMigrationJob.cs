using ProductAggregate.API.Domain.ProductAggregate;

namespace ProductAggregate.API.Application.ConsistentHashing
{
    public interface IProductMigrationJob
    {
        [AutomaticRetry(Attempts = 0)]
        Task<string> ProcessFileMigrationAsync();
    }

    public class ProdcutMigrationJob : IProductMigrationJob, ITransient
    {
        private readonly IHashRingManager _hashRingManager;
        private readonly IServiceProvider _serviceProvider;
        public ProdcutMigrationJob(
            IHashRingManager hashRingManager,
            IServiceProvider serviceProvider)
        {
            _hashRingManager = hashRingManager;
            _serviceProvider = serviceProvider;
        }

        public async Task<string> ProcessFileMigrationAsync()
        {
            await _hashRingManager.TryInit();

            var serializeOptions = new JsonSerializerOptions() 
            { 
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
            };
            var filePaths = Directory.GetFiles(@"D:\Dataset\extract", "*.jsonl");
            ConcurrentDictionary<Server, List<ProductItem>> productServer = [];

            SemaphoreSlim semaphore = new(300);
            List<Task> processTasks = [];
            List<Task> addTasks = [];
            List<string> rs = [];
            foreach (var filePath in filePaths)
            {
                using (var reader = new StreamReader(filePath))
                {
                    string? line;
                    int count = 0;
                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        count++;
                        var migrateProduct = JsonSerializer.Deserialize<Domain.ProductMigration.ProductItem>(line, serializeOptions);
                        if (migrateProduct == null) return "";

                        var product = BsonSerializer.Deserialize<ProductItem>(JsonSerializer.Serialize(migrateProduct));
                        var hashedVNode = _hashRingManager.HashRing.GetBucket(product.Id.ToString());
                        var vNode = productServer.Keys.SingleOrDefault(x => x == hashedVNode.Node);

                        product.VirtualId = hashedVNode.VirtualId;
                        if (vNode is null)
                            productServer.TryAdd(hashedVNode.Node, []);

                        productServer[hashedVNode.Node].Add(product);

                        if (count % 10_000 == 0)
                        {
                            await Task.WhenAll(processTasks);
                            foreach (var item in productServer)
                            {
                                //addTasks.Add(AddServerProductAsync(item.Key, item.Value));
                                await AddServerProductAsync(item.Key, item.Value);
                            }
                            //await Task.WhenAll(addTasks);
                            productServer.Clear();
                        }
                    }

                    await Task.WhenAll(processTasks);
                    foreach (var item in productServer)
                    {
                        //addTasks.Add(AddServerProductAsync(item.Key, item.Value));
                        await AddServerProductAsync(item.Key, item.Value);
                    }
                    await Task.WhenAll(addTasks);
                    rs.Add($"File {filePath}, processed: {count}");
                }
            }

            return string.Join("/n", rs);
        }

        private async Task AddServerProductAsync(Server server, IEnumerable<ProductItem> productItems)
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            //TODO: Hidden mongo connection logic
            context.SetConnection($"mongodb://admin:1234@127.0.0.1:{server.Port}");
            context.SetDatabase(server.Database);
            var collection = context.Collection<ProductItem>();
            await collection.InsertManyAsync(productItems);
        }
    }
}
