using Core.AspNet.Result;
using Core.AppResults;
using FastEndpoints;
using MediatR;
using Ordering.API.Application.Services;

namespace Ordering.API.Presentation.Endpoint
{
    public class ConfirmStock : EndpointWithoutRequest<HttpResultTyped<AppResult<string>>>
    {
		private readonly IMediator _mediator;

        public ConfirmStock(IMediator mediator)
        {
            _mediator = mediator;
        }
      
        public override void Configure()
		{
			Post("ordering/{id}/confirm-stock");
			//AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var id = Route<Guid>("id");
            var request = new ConfirmStockRequest() { OrderId = id };
            var result = await _mediator.Send(request, ct);
            await SendResultAsync(result.ToHttpResult());
        }
    }
}
