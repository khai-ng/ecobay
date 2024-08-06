using FastEndpoints;
namespace Ordering.API.Presentation.Endpoint
{
	public class AddOrder: EndpointWithoutRequest
	{
		public override void Configure()
		{
			Post("order/add");
			AllowAnonymous();
		}

		public override async Task HandleAsync(CancellationToken ct)
		{
			await SendResultAsync(Results.Ok());
		}
	}
}
