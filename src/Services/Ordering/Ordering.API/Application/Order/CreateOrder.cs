namespace Ordering.API.Application.Services
{
    public class CreateOrder : IRequestHandler<CreateOrderCommand, AppResult<Guid>>, ITransient
    {
        private readonly IEventStoreRepository<Order> _orderRepository;

        public CreateOrder(IEventStoreRepository<Order> orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<AppResult<Guid>> Handle(CreateOrderCommand request, CancellationToken ct)
        {
            var address = new Address(request.Country, request.City, request.District, request.Street);
            var orderItems = request.OrderItems.Select(x => new OrderItem(x.ProductId, x.Price, x.Qty));
            var order = new Order(request.BuyerId, request.PaymentId, address, orderItems);

            await _orderRepository.AddAsync(order.Id, order, ct).ConfigureAwait(false);

            return AppResult.Success(order.Id); 
        }
    }
}
