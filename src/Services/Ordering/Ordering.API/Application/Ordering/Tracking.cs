namespace Ordering.API.Application.Ordering
{
    public class Tracking : IRequestHandler<TrackingCommand, AppResult<IEnumerable<TrackingResponse>>>, ITransient
    {
        private readonly IOrderRepository _orderRepository;

        public Tracking(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<AppResult<IEnumerable<TrackingResponse>>> HandleAsync(TrackingCommand request, CancellationToken cancellationToken)
        {
            var stream = await _orderRepository.GetStreamAsync(request.Id);
            return AppResult.Success(stream.Select(x => new TrackingResponse(x.Id, x.TypeName, x.Sequence, x.Version, x.CreatedAt)));
        }
    }
}
