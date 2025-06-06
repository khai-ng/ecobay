namespace Ordering.API.Presentation.Endpoints
{
	public class Get : Endpoint<GetOrderCommand, HttpResultTyped<AppResult<IEnumerable<OrderView>>>>
	{
		private readonly IMediator _mediator;
		public Get(IMediator mediator)
		{
			_mediator = mediator;
		}

		public override void Configure() => Get("orders");

		public override async Task HandleAsync(GetOrderCommand req, CancellationToken ct)
		{
			var result = await _mediator.Send(req, ct).ConfigureAwait(false);
			await SendResultAsync(result.ToHttpResult()).ConfigureAwait(false);
		}
	}
}
