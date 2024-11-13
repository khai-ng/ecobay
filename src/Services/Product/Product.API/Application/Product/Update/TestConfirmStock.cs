using Core.AppResults;
using Core.SharedKernel;
using MediatR;
using MongoDB.Bson;
using Product.API.Application.Common.Abstractions;
using Product.API.Application.Product.Get;

namespace Product.API.Application.Product.Update
{
    public class ConfirmStockHandler : IRequestHandler<ConfirmStockRequest, AppResult>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ConfirmStockHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }


        public async Task<AppResult> Handle(ConfirmStockRequest request, CancellationToken ct)
        {
            var productRequest = new GetProductByIdRepoRequest(
                "product-1",
                new List<ObjectId> { ObjectId.Parse(request.Id) });
            var product = (await _productRepository.GetAsync(productRequest)).Single();

            if (product == null)
                return AppResult.NotFound($"Not found product: {request.Id}");

            if (request.Units > product.Units)
                return AppResult.Invalid(new ErrorDetail(nameof(ArgumentOutOfRangeException)));

            product.Units -= request.Units;

            _productRepository.Update(product);
            await _unitOfWork.SaveChangesAsync(ct);

            return AppResult.Success();
        }
        
    }
}
