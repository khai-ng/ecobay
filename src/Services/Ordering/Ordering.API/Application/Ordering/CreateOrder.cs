namespace Ordering.API.Application.Ordering
{
    public class CreateOrder : IRequestHandler<CreateOrderCommand, AppResult<Guid>>, ITransient
    {
        private readonly IEventStoreRepository<Order> _orderRepository;
        private readonly IUser _user;

        public CreateOrder(IEventStoreRepository<Order> orderRepository, IUser user)
        {
            _orderRepository = orderRepository;
            _user = user;
        }

        public async Task<AppResult<Guid>> Handle(CreateOrderCommand request, CancellationToken ct)
        {
            Guid buyerId;
            if(request.BuyerId != null)
            {
                var user = _user.Info();
                buyerId = user.Id;
            } else
                buyerId = (Guid)request.BuyerId!;

            var address = new Address(request.Country, request.City, request.District, request.Street);
            var orderItems = request.OrderItems.Select(x => new OrderItem(x.ProductId, x.Price, x.Qty));
            var order = new Order(buyerId, request.PaymentId, address, orderItems);

            await _orderRepository.AddAsync(order.Id, order, ct).ConfigureAwait(false);

            return AppResult.Success(order.Id);
        }
    }
}
