using Core.Result.AppResults;
using Core.SharedKernel;
using MediatR;
using MongoDB.Bson;
using Product.API.Application.Common.Abstractions;

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


        public async Task<AppResult> Handle(ConfirmStockRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var ids = new List<ObjectId> { ObjectId.Parse(request.Id) };
                var product = (await _productRepository.GetAsync(ids)).Single();

                if (request.Units > product.Units)
                    throw new ArgumentOutOfRangeException(nameof(ConfirmStockRequest));

                product.Units -= request.Units;

                _productRepository.Update(product);
                await _unitOfWork.SaveChangesAsync();

                return AppResult.Success();
            }
            catch (Exception ex)
            {
                return AppResult.Error(ex.Message);
            }
        }
    }
}
