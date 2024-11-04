using Core.Autofac;
using Core.Events.EventStore;
using Core.AppResults;
using Core.SharedKernel;
using Marten.Events;
using MediatR;
using Ordering.API.Application.Common.Abstractions;
using Ordering.API.Domain.OrderAggregate;

namespace Ordering.API.Application.Services
{
    public class CreateOrder : IRequestHandler<CreateOrderRequest, AppResult<Guid>>, ITransient
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderRepository _orderRepository;
        private readonly IEventStoreRepository<Order> _eventStoreRepository;

        public CreateOrder(IUnitOfWork unitOfWork, 
            IOrderRepository orderRepository, 
            IEventStoreRepository<Order> eventStoreRepository)
        {
            _unitOfWork = unitOfWork;
            _orderRepository = orderRepository;
            _eventStoreRepository = eventStoreRepository;
        }

        public async Task<AppResult<Guid>> Handle(CreateOrderRequest request, CancellationToken ct)
        {
            var address = new Address(request.Country, request.City, request.District, request.Street);
            var orderItems = request.OrderItems.Select(x => new OrderItem(x.ProductId, x.UnitPrice, x.Unit));
            var order = new Order(request.BuyerId, request.PaymentId, address, orderItems);
            _orderRepository.Add(order);

            await _eventStoreRepository.Add(order.Id, order, ct).ConfigureAwait(false);
            await _unitOfWork.SaveChangesAsync(ct).ConfigureAwait(false);

            return AppResult.Success(order.Id); 
        }
    }
}
