namespace ProductAggregate.API.Presentation.Endpoint
{
    public class GetProductEndpoint : Endpoint<GetProductRequest, HttpResultTyped<PagingResponse<ProductItemDto>>>
    {
        private readonly IMediator _mediator;
        public GetProductEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }
        public override void Configure()
        {
            Get("product");
            //AllowAnonymous();
        }

        public override async Task HandleAsync(GetProductRequest request, CancellationToken ct)
        {
            var result = await _mediator.Send(request, ct);
            await SendResultAsync(result.ToHttpResult());
        }
    }
}
