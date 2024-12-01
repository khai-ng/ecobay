using ProductAggregate.API.Domain.ProductAggregate;

namespace ProductAggregate.API.Application.Product.GetProduct
{
    public class GetProductById : IRequestHandler<GetProductByIdRequest, AppResult<IEnumerable<ProductItemDto>>>, ITransient
    {
        private readonly Serilog.ILogger _logger;
        private readonly IProductHashingService _productHashingService;
        private readonly IProductRepository _productRepository;

        public GetProductById(Serilog.ILogger logger,
            IProductHashingService productHashingService,
            IProductRepository productRepository)
        {
            _logger = logger;
            _productHashingService = productHashingService;
            _productRepository = productRepository;
        }

        public async Task<AppResult<IEnumerable<ProductItemDto>>> Handle(GetProductByIdRequest request, CancellationToken ct)
        {
            var productHashingRequest = request.Ids.Select(x => new ProductHashingDto(x));
            var listProductHashed = await _productHashingService.HashProductAsync(productHashingRequest);

            List<Task<AppResult<IEnumerable<ProductItem>>>> tasks = [];
            foreach (var productHashed in listProductHashed)
            {
                var server = productHashed.Key.Node;
                var channel = _productHashingService.TryGetChannel(server.Host);
                if (channel == null)
                {
                    _logger.ForContext(typeof(GetProductByIdRequest))
                        .Fatal("The matching resource server not found");

                    return AppResult.Error("Order product not found");
                }

                var repoRequest = new GetProductByIdRepoRequest(
                    server.Database,
                    channel,
                    productHashed.Value.Select(x => x.Id));

                tasks.Add(_productRepository.GetByIdAsync(repoRequest));
            }

            var taskResults = await Task.WhenAll(tasks);

            var result = taskResults
                .SelectMany(x => x.Data ?? [])
                .Select(x => new ProductItemDto()
                {
                    MainCategory = x.MainCategory,
                    Title = x.Title,
                    Price = x.Price,
                });
            return AppResult.Success(result);

        }
    }

    internal class ProductHashingDto(string id) : IHashable
    {
        public string Id { get; set; } = id;
    }
}
