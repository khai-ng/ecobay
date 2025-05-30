namespace Ordering.API.Application.Ordering
{
	public class GetOrder : IRequestHandler<GetOrderCommand, AppResult<IEnumerable<OrderView>>>, ITransient
	{
		private readonly IOrderRepository _orderRepository;

        public GetOrder(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<AppResult<IEnumerable<OrderView>>> Handle(GetOrderCommand request, CancellationToken cancellationToken)
		{
            var res = await _orderRepository.GetAsync(request.Id, request.BuyerId, request.OrderStatusId);
            return AppResult.Success(res);
        }
	}
}
