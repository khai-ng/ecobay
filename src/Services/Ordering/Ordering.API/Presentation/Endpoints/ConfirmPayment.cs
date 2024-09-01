using Core.AspNet.Result;
using Core.Result.AppResults;
using FastEndpoints;
using MediatR;
using Ordering.API.Application.Services;

namespace Ordering.API.Presentation.Endpoint
{
    public class ConfirmPayment : Endpoint<ConfirmPaymentRequest, HttpResultTyped<AppResult<string>>>
    {
		private readonly IMediator _mediator;

        public ConfirmPayment(IMediator mediator)
        {
            _mediator = mediator;
        }
      
        public override void Configure()
		{
			Get("order/confirm-payment");
			AllowAnonymous();
        }

        public override async Task HandleAsync(ConfirmPaymentRequest request,  CancellationToken ct)
        {
            var result = await _mediator.Send(request, ct);
            await SendResultAsync(result.ToHttpResult());
        }
    }
}
