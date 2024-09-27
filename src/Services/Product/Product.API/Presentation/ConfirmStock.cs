using Core.AspNet.Result;
using Core.AppResults;
using FastEndpoints;
using MediatR;
using Product.API.Application.Product.Update;

namespace Product.API.Presentation
{
    public class ConfirmStockEndpoint : Endpoint<ConfirmStockRequest, HttpResultTyped<AppResult>>
    {
        private readonly IMediator _mediator;

        public ConfirmStockEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Post("product/confirm-stock");
            AllowAnonymous();
        }

        public override async Task HandleAsync(ConfirmStockRequest req, CancellationToken ct)
        {
            var result = await _mediator.Send(req, ct);
            await SendResultAsync(result.ToHttpResult());
        }
    }
}
