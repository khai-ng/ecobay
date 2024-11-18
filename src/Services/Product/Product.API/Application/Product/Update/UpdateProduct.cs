using Core.SharedKernel;
using Grpc.Core;
using GrpcProduct.Update;
using MongoDB.Bson;
using Product.API.Application.Common.Abstractions;
using Product.API.Application.Product.Get;

namespace Product.API.Application.Product.Update
{
    public class UpdateProduct: UpdateProductService.UpdateProductServiceBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProduct(IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public override async Task<UpdateProductUnitResponse> ConfirmStock(
            GrpcProduct.Update.ConfirmStockRequest request, 
            ServerCallContext context)
        {
            try
            {
                var productUnitReq = request.ProductUnits.ToList();
                var productRequest = new GetProductByIdRepoRequest(
                    request.DbName, 
                    request.ProductUnits.Select(x => ObjectId.Parse(x.Id)));

                var productItems = await _productRepository.GetAsync(productRequest);

                foreach (var req in productUnitReq)
                {
                    var product = productItems.Single(x => x.Id == ObjectId.Parse(req.Id));
                    if (product == null)
                        return new UpdateProductUnitResponse() { IsSuccess = false, Message = $"Product {req.Id} not found" };

                    if (req.Units > product.Units)
                        return new UpdateProductUnitResponse() { IsSuccess = false, Message = $"Product {req.Id} invalid unit {req.Units}" };

                    product.Units -= req.Units;
                }
                _productRepository.UpdateRange(productItems);
                await _unitOfWork.SaveChangesAsync();

                return new UpdateProductUnitResponse() { IsSuccess = true };
            }
            catch (Exception ex)
            {
                return new UpdateProductUnitResponse()
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
            
        }
    }
}
