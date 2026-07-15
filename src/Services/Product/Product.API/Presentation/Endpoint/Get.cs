namespace Product.API.Presentation.Endpoint
{
    public class Get : Endpoint<GetProductCommand, HttpResultTyped<PagingResponse<ProductItemDto>>>
    {
        private readonly IMediator _mediator;
        public Get(IMediator mediator)
        {
            _mediator = mediator;
        }
        public override void Configure()
        {
            Get("products");
            AllowAnonymous();
        }

        public override async Task HandleAsync(GetProductCommand req, CancellationToken ct)
        {
            var result = await _mediator.PublishAsync(req, ct).ConfigureAwait(false);
            await Send.ResultAsync(result.ToHttpResult()).ConfigureAwait(false);
        }
    }
}
