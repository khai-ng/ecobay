using System.Text.Json;

namespace Product.API.Application.Product.Migration
{
    public interface IProductMigrationJob
    {
        [AutomaticRetry(Attempts = 0)]
        Task<string> ProcessFileMigrationAsync();
    }

    public class ProdcutMigrationJob : IProductMigrationJob, ITransient
    {
        private readonly AppDbContext _appDbContext;

        public ProdcutMigrationJob(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<string> ProcessFileMigrationAsync()
        {
            var serializeOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
            };

            var filePaths = Directory.GetFiles(@"D:\Dataset\extract", "*.jsonl");
            List<string> result = [];
            List<ProductItem> addList = [];

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
                        addList.Add(product);
                        if (count % 10_000 == 0)
                        {
                            await AddServerProductAsync(addList);
                            addList.Clear();
                        }
                    }
                    await AddServerProductAsync(addList);

                    result.Add($"File {filePath}, processed: {count}");
                }
            }

            return string.Join("/n", result);
        }

        private async Task AddServerProductAsync(IEnumerable<ProductItem> productItems)
        {
            var collection = _appDbContext.Collection<ProductItem>();
            await collection.InsertManyAsync(productItems);
        }
    }
}
