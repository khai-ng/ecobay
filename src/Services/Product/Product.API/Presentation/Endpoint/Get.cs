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

        public override async Task HandleAsync(GetProductCommand request, CancellationToken ct)
        {
            var result = await _mediator.Send(request, ct).ConfigureAwait(false);
            await SendResultAsync(result.ToHttpResult()).ConfigureAwait(false);
        }
    }
}
