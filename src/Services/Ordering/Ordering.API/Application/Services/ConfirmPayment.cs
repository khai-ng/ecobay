using Core.Autofac;
using Core.Result.AppResults;
using Core.SharedKernel;
using MediatR;
using Ordering.API.Application.Abstractions;
using Ordering.API.Infrastructure.Repositories;

namespace Ordering.API.Application.Services
{
    public class ConfirmPayment : IRequestHandler<ConfirmPaymentRequest, AppResult>, ITransient
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ConfirmPayment(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<AppResult> Handle(ConfirmPaymentRequest request, CancellationToken ct)
        {
            var order = await _orderRepository.FindAsync(request.OrderId);

            if (order == null)
                return AppResult.Invalid(new ErrorDetail($"Can not find order: {request.OrderId}"));

            order.SetPaid();
            _orderRepository.Update(order);
            await _unitOfWork.SaveChangesAsync(ct);

            return AppResult.Success();
        }
    }
}
