namespace Product.API.Presentation.Endpoint
{
    public class GetById : EndpointWithoutRequest<HttpResultTyped<IEnumerable<ProductItemDto>>>
    {
        private readonly IMediator _mediator;
        public GetById(IMediator mediator)
        {
            _mediator = mediator;
        }
        public override void Configure()
        {
            Get("products/{id}");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            string[] ids = [Route<string>("id")!];
            var request = new GetProductByIdCommand(ids);
            var result = await _mediator.Send(request, ct).ConfigureAwait(false);
            await Send.ResultAsync(result.ToHttpResult()).ConfigureAwait(false);
        }
    }
}
