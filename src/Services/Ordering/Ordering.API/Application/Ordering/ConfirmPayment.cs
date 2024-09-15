using Core.Autofac;
using Core.Events.EventStore;
using Core.Result.AppResults;
using Core.SharedKernel;
using Marten.Events;
using MediatR;
using Ordering.API.Application.Common.Abstractions;
using Ordering.API.Domain.OrderAggregate;

namespace Ordering.API.Application.Services
{
    public class ConfirmPayment : IRequestHandler<ConfirmPaymentRequest, AppResult<string>>, ITransient
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEventStoreRepository<Order> _eventStoreRepository;

        public ConfirmPayment(IOrderRepository orderRepository, IUnitOfWork unitOfWork, IEventStoreRepository<Order> eventStoreRepository)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
            _eventStoreRepository = eventStoreRepository;
        }

        public async Task<AppResult<string>> Handle(ConfirmPaymentRequest request, CancellationToken ct)
        {
            var order = await _orderRepository.FindAsync(request.OrderId).ConfigureAwait(false);

            if (order == null)
                return AppResult.Invalid(new ErrorDetail($"Can not find order {request.OrderId}"));

            order.SetPaid();
            _orderRepository.Update(order);

            await _eventStoreRepository.Update(order.Id, order, order.Version, ct: ct).ConfigureAwait(false);
            await _unitOfWork.SaveChangesAsync(ct).ConfigureAwait(false);

            return AppResult.Success("Successful");
        }
    }
}
