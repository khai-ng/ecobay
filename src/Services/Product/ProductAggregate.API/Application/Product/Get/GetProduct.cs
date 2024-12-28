namespace ProductAggregate.API.Application.Product.GetProduct
{
    public class GetProductHandler : IRequestHandler<GetProductRequest, AppResult<PagingResponse<ProductItemDto>>>, ITransient
    {
        private readonly Serilog.ILogger _logger;
        private readonly IProductRepository _productRepository;
        private readonly IProductHashingService _productHashingService;
        private readonly IServerRepository _serverRepository;

        public GetProductHandler(
            Serilog.ILogger logger,
            IProductRepository productRepository,
            IProductHashingService productHashingService,
            IServerRepository serverRepository)
        {
            _logger = logger;
            _productRepository = productRepository;
            _productHashingService = productHashingService;
            _serverRepository = serverRepository;
        }

        public async Task<AppResult<PagingResponse<ProductItemDto>>> Handle(
            GetProductRequest request,
            CancellationToken ct)
        {
            List<Task<AppResult<GetProductRepoResponse>>> tasks = [];

            //TODO: implement batching get product mechanism 
            var listDatabase = (await _serverRepository.GetAllAsync())
                .Select(x => new
                {
                    x.Host,
                    x.Port,
                    x.Database
                })
                .Distinct()
                .ToList();
            foreach (var item in listDatabase)
            {
                var channel = _productHashingService.TryGetChannel(item.Host);
                if (channel == null) { continue; }

                var channelRequest = new GetProductRepoRequest()
                {
                    Channel = channel,
                    DbName = item.Database,
                    Category = request.Category,
                    PageIndex = request.PageIndex,
                    PageSize = Convert.ToInt32(Math.Ceiling((decimal)request.PageSize / listDatabase.Count)),
                };
                tasks.Add(_productRepository.GetAsync(channelRequest));
            }

            var taskResults = await Task.WhenAll(tasks);
            var data = taskResults.SelectMany(x => x.Data?.ProductItems ?? [])
                .Take(request.PageSize)
                .Select(x => new ProductItemDto()
                {
                    MainCategory = x.MainCategory,
                    Title = x.Title,
                    AverageRating = x.AverageRating,
                    RatingNumber = x.RatingNumber,
                    Price = x.Price,
                    Images = x.Images,
                    Videos = x.Videos,
                    Store = x.Store,
                    Categories = x.Categories,
                    Details = x.Details
                });

            var rs = FluentPaging
                .From(request)
                .Result(data);

            rs.HasNext = taskResults.Any(x => (x.Data?.HasNext ?? false) == true);

            return rs;
        }
    }
}
