namespace Ordering.API.Application.Ordering
{
    public class ConfirmPayment : IRequestHandler<ConfirmPaymentCommand, AppResult<string>>, ITransient
    {
        private readonly IEventStoreRepository<Order> _orderRepository;

        public ConfirmPayment(IEventStoreRepository<Order> eventStoreRepository)
        {
            _orderRepository = eventStoreRepository;
        }

        public async Task<AppResult<string>> HandleAsync(ConfirmPaymentCommand request, CancellationToken ct)
        {
            var order = await _orderRepository.FindAsync(request.OrderId, ct).ConfigureAwait(false);

            if (order == null)
                return AppResult.Invalid(new ErrorDetail($"Can not find order {request.OrderId}"));

            order.SetPaid();
            await _orderRepository.UpdateAsync(order.Id, order, order.Version, ct).ConfigureAwait(false);

            return AppResult.Success("Successful");
        }
    }
}
