using Core.Autofac;
using Core.Result.AppResults;
using Core.SharedKernel;
using MediatR;
using Ordering.API.Application.Abstractions;
using Ordering.API.Domain.OrderAggregate;
using Ordering.API.Domain.OrderAgrregate;

namespace Ordering.API.Application.Services
{
    public class CreateOrder : IRequestHandler<CreateOrderRequest, AppResult<CreateOrderResponse>>, ITransient
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateOrder(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<AppResult<CreateOrderResponse>> Handle(CreateOrderRequest request, CancellationToken ct)
        {
            var orderId = Guid.NewGuid();
            var address = new Address(request.Country, request.City, request.District, request.Street);
            var orderItems = request.OrderItems.Select(x => new OrderItem(orderId, x.ProductId, x.UnitPrice, x.Unit));
            var order = new Order(request.BuyerId, request.PaymentId, address, orderItems);

            _orderRepository.Add(order);
            await _unitOfWork.SaveChangesAsync(ct);

            return AppResult.Success(new CreateOrderResponse(order.Id));
        }
    }
}
