namespace Ordering.API.Presentation.Endpoint
{
    public class ConfirmStock : EndpointWithoutRequest<HttpResultTyped<AppResult<string>>>
    {
		private readonly IMediator _mediator;

        public ConfirmStock(IMediator mediator)
        {
            _mediator = mediator;
        }
      
        public override void Configure()
		{
			Post("order/{id}/confirm-stock");
			//AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var request = new ConfirmStockCommand(Route<Guid>("id"));
            var result = await _mediator.Send(request, ct).ConfigureAwait(false);
            await SendResultAsync(result.ToHttpResult()).ConfigureAwait(false);
        }
    }
}
