using Core.AspNet.Result;
using Core.AppResults;
using FastEndpoints;
using MediatR;
using ProductAggregate.API.Application.Product;
using ProductAggregate.API.Application.Product.GetProduct;

namespace ProductAggregate.API.Presentation.Endpoint
{
    public class GetProductByIdEndpoint : Endpoint<string, HttpResultTyped<AppResult<IEnumerable<ProductItemDto>>>>
    {
        private readonly IMediator _mediator;
        public GetProductByIdEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }
        public override void Configure()
        {
            Get("product/{id}");
            //AllowAnonymous();
        }

        public override async Task HandleAsync(string id, CancellationToken ct)
        {
            var request = new GetProductByIdRequest() { Ids = new[] { id } };
            var result = await _mediator.Send(request, ct);
            await SendResultAsync(result.ToHttpResult());
        }
    }
}
