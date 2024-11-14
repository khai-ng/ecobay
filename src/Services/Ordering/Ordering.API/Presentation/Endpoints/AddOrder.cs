using Core.AspNet.Result;
using Core.AppResults;
using FastEndpoints;
using MediatR;
using Ordering.API.Application.Services;

namespace Ordering.API.Presentation.Endpoint
{
    public class AddOrder : Endpoint<CreateOrderRequest, HttpResultTyped<AppResult<Guid>>>
    {
		private readonly IMediator _mediator;

        public AddOrder(IMediator mediator)
        {
            _mediator = mediator;
        }
      
        public override void Configure()
		{
			Post("ordering/add");
			//AllowAnonymous();
        }

        public override async Task HandleAsync(CreateOrderRequest req,  CancellationToken ct)
        {
            var result = await _mediator.Send(req, ct);
            await SendResultAsync(result.ToHttpResult());
        }
    }
}
