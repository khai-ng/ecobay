namespace Ordering.API.Presentation.Endpoint
{
    public class ConfirmPayment : EndpointWithoutRequest<HttpResultTyped<AppResult<string>>>
    {
		private readonly IMediator _mediator;

        public ConfirmPayment(IMediator mediator)
        {
            _mediator = mediator;
        }
      
        public override void Configure()
		{
			Post("order/{id}/confirm-payment");
			//AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var request = new ConfirmPaymentCommand(Route<Guid>("id"));
            var result = await _mediator.Send(request, ct).ConfigureAwait(false);
            await SendResultAsync(result.ToHttpResult()).ConfigureAwait(false);
        }
    }
}
