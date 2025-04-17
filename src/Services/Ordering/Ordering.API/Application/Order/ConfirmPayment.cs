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

            if (order.OrderStatus != OrderStatus.StockConfirmed)
                return AppResult.Invalid(new ErrorDetail(nameof(order.OrderStatus), $"Order must be {OrderStatus.StockConfirmed.Name}"));

            order.SetPaid();
            _orderRepository.Update(order);

            await _eventStoreRepository.Update(order.Id, order, order.Version, ct).ConfigureAwait(false);
            await _unitOfWork.SaveChangesAsync(ct).ConfigureAwait(false);

            return AppResult.Success("Successful");
        }
    }
}
