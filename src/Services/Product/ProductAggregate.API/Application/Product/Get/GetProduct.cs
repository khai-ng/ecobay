using Core.Autofac;
using Core.Result.AppResults;
using Core.Result.Paginations;
using MediatR;
using ProductAggregate.API.Application.Common.Abstractions;
using ProductAggregate.API.Application.Product.Get;

namespace ProductAggregate.API.Application.Product.GetProduct
{
    public class GetProductHandler : IRequestHandler<GetProductRequest, AppResult<PagingResponse<ProductItemDto>>>, ITransient
    {
        private readonly Serilog.ILogger _logger;
        private readonly IProductRepository _productRepository;
        private readonly IProductHashingService _productHashingService;

        public GetProductHandler(
            Serilog.ILogger logger,
            IProductRepository productRepository,
            IProductHashingService productHashingService)
        {
            _logger = logger;
            _productRepository = productRepository;
            _productHashingService = productHashingService;
        }

        public async Task<AppResult<PagingResponse<ProductItemDto>>> Handle(
            GetProductRequest request,
            CancellationToken ct)
        {
            List<Task<AppResult<GetProductRepoResponse>>> tasks = [];

            //TODO: implement batching get product mechanism 
            foreach (var item in _productHashingService.GetAllChannel())
            {
                var channelRequest = new GetProductRepoRequest()
                {
                    Channel = item,
                    Category = request.Category,
                    PageIndex = request.PageIndex,
                    PageSize = Convert.ToInt32(Math.Ceiling((decimal)request.PageSize / _productHashingService.ChannelUnits)),

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
                    Price = x.Price,
                });

            var rs = FluentPaging
                .From(request)
                .Result(data);

            rs.HasNext = taskResults.Any(x => (x.Data?.HasNext ?? false) == true);

            return rs;
        }
    }
}
