namespace Ordering.API.Presentation.Endpoints
{
	public class TrackingEndpoint: EndpointWithoutRequest<HttpResultTyped<AppResult<string>>>
	{
		private readonly IMediator _mediator;

		public TrackingEndpoint(IMediator mediator)
		{
			_mediator = mediator;
		}

		public override void Configure()
		{
			Get("orders/{id}/tracking");
			//AllowAnonymous();
		}

		public override async Task HandleAsync(CancellationToken ct)
		{
			var request = new TrackingCommand(Route<Guid>("id"));
			var result = await _mediator.Send(request, ct).ConfigureAwait(false);
			await Send.ResultAsync(result.ToHttpResult()).ConfigureAwait(false);
		}

	}
}
