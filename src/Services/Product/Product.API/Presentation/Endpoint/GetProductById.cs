namespace Product.API.Presentation.Endpoint
{
    public class GetProductByIdEndpoint : EndpointWithoutRequest<HttpResultTyped<IEnumerable<ProductItemDto>>>
    {
        private readonly IMediator _mediator;
        public GetProductByIdEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }
        public override void Configure()
        {
            Get("product/{id}");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            string?[] ids = [Route<string>("id")];
            var request = new GetProductByIdCommand(ids);
            var result = await _mediator.Send(request, ct).ConfigureAwait(false);
            await SendResultAsync(result.ToHttpResult()).ConfigureAwait(false);
        }
    }
}
