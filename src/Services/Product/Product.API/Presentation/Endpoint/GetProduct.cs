namespace Product.API.Presentation.Endpoint
{
    public class GetProductEndpoint : Endpoint<GetProductQuery, HttpResultTyped<PagingResponse<ProductItemDto>>>
    {
        private readonly IMediator _mediator;
        public GetProductEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }
        public override void Configure()
        {
            Get("product");
            AllowAnonymous();
        }

        public override async Task HandleAsync(GetProductQuery request, CancellationToken ct)
        {
            var result = await _mediator.Send(request, ct).ConfigureAwait(false);
            await SendResultAsync(result.ToHttpResult()).ConfigureAwait(false);
        }
    }
}
