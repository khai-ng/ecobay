using Core.AspNet.Result;
using Core.AppResults;
using FastEndpoints;
using MediatR;
using Ordering.API.Application.Services;

namespace Ordering.API.Presentation.Endpoint
{
    public class ConfirmPayment : Endpoint<Guid, HttpResultTyped<AppResult<string>>>
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

        public override async Task HandleAsync(Guid id,  CancellationToken ct)
        {
            var request = new ConfirmPaymentRequest() { OrderId = id };
            var result = await _mediator.Send(request, ct);
            await SendResultAsync(result.ToHttpResult());
        }
    }
}
