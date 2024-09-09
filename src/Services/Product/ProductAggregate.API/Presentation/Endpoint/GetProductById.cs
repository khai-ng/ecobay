using Core.AspNet.Result;
using Core.Result.AppResults;
using FastEndpoints;
using MediatR;
using ProductAggregate.API.Application.Product;
using ProductAggregate.API.Application.Product.GetProduct;

namespace ProductAggregate.API.Presentation.Endpoint
{
    public class GetProductByIdEndpoint : Endpoint<GetProductByIdRequest, HttpResultTyped<AppResult<IEnumerable<ProductItemDto>>>>
    {
        private readonly IMediator _mediator;
        public GetProductByIdEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }
        public override void Configure()
        {
            Post("product/get-by-id");
            AllowAnonymous();
        }

        public override async Task HandleAsync(GetProductByIdRequest request, CancellationToken ct)
        {
            var result = await _mediator.Send(request, ct);
            await SendResultAsync(result.ToHttpResult());
        }
    }
}
