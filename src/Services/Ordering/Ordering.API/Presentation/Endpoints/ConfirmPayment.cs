using Core.AspNet.Result;
using Core.AppResults;
using FastEndpoints;
using MediatR;
using Ordering.API.Application.Services;

namespace Ordering.API.Presentation.Endpoint
{
    public class ConfirmPayment : EndpointWithoutRequest<HttpResultTyped<AppResult<string>>>
    {
		private readonly IMediator _mediator;

        public ConfirmPayment(IMediator mediator)
        {
            _mediator = mediator;
        }
      
        public override void Configure()
		{
			Post("ordering/{id}/confirm-payment");
			//AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var id = Route<Guid>("id");
            var request = new ConfirmPaymentRequest() { OrderId = id };
            var result = await _mediator.Send(request, ct);
            await SendResultAsync(result.ToHttpResult());
        }
    }
}
