using FastEndpoints;
using System.Security.Claims;

namespace Web.ApiGateway.Endpoint
{
	public class Me: EndpointWithoutRequest<object>
	{
		private readonly IHttpContextAccessor _principal;
		public Me(IHttpContextAccessor principal)
		{
			_principal = principal;
		}

		public override void Configure()
		{
			Get("user/me");
		}

		public override async Task HandleAsync(CancellationToken ct)
		{
			await SendAsync(_principal.HttpContext);
		}
	}
}
