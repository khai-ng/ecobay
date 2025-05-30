namespace Ordering.API.Presentation.Endpoint
{
    public class Add : Endpoint<CreateOrderCommand, HttpResultTyped<AppResult<Guid>>>
    {
		private readonly IMediator _mediator;

        public Add(IMediator mediator)
        {
            _mediator = mediator;
        }
      
        public override void Configure() => Post("order");

        public override async Task HandleAsync(CreateOrderCommand req,  CancellationToken ct)
        {
            var result = await _mediator.Send(req, ct).ConfigureAwait(false);
            await SendResultAsync(result.ToHttpResult()).ConfigureAwait(false);
        }
    }
}
