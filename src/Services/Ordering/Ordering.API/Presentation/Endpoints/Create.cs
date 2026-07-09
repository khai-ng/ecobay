namespace Ordering.API.Presentation.Endpoint
{
    public class Create : Endpoint<CreateOrderCommand, HttpResultTyped<AppResult<Guid>>>
    {
		private readonly IMediator _mediator;

        public Create(IMediator mediator)
        {
            _mediator = mediator;
        }
      
        public override void Configure() => Post("orders");

        public override async Task HandleAsync(CreateOrderCommand req,  CancellationToken ct)
        {
            var result = await _mediator.Publish(req, ct).ConfigureAwait(false);
            await Send.ResultAsync(result.ToHttpResult()).ConfigureAwait(false);
        }
    }
}