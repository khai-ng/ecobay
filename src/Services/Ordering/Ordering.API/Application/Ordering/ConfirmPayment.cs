using Core.AppResults;
using Core.Autofac;
using Core.Events.EventStore;
using Core.SharedKernel;
using MediatR;
using Ordering.API.Application.Common.Abstractions;
using Ordering.API.Domain.OrderAggregate;

namespace Ordering.API.Application.Services
{
    public class ConfirmPayment : IRequestHandler<ConfirmPaymentRequest, AppResult<string>>, ITransient
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderRepository _orderRepository;
        private readonly IEventStoreRepository<Order> _eventStoreRepository;

        public ConfirmPayment(IUnitOfWork unitOfWork, 
            IOrderRepository orderRepository, 
            IEventStoreRepository<Order> eventStoreRepository)
        {
            _unitOfWork = unitOfWork;
            _orderRepository = orderRepository;
            _eventStoreRepository = eventStoreRepository;
        }

        public async Task<AppResult<string>> Handle(ConfirmPaymentRequest request, CancellationToken ct)
        {
            var order = await _orderRepository.FindAsync(request.OrderId).ConfigureAwait(false);

            if (order == null)
                return AppResult.Invalid(new ErrorDetail($"Can not find order {request.OrderId}"));

            order.SetPaid();
            _orderRepository.Update(order);

            await _eventStoreRepository.Update(order.Id, order, order.Version, ct).ConfigureAwait(false);
            await _unitOfWork.SaveChangesAsync(ct).ConfigureAwait(false);

            return AppResult.Success("Successful");
        }
    }
}
