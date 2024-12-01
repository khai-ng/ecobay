
namespace Web.ApiGateway.Endpoint
{
    public class Me: EndpointWithoutRequest<HttpResultTyped<Dictionary<string, string>>>
	{
		private readonly IHttpContextAccessor _httpContext;
		public Me(IHttpContextAccessor httpContext)
		{
            _httpContext = httpContext;
		}

		public override void Configure()
		{
			Get("user/me");
		}

        public override async Task HandleAsync(CancellationToken ct)
        {
            var claim = _httpContext.HttpContext?.User.Claims.ToDictionary(x => x.Type, x => x.Value) ?? [];
            var appResult = AppResult.Success(claim);
            await SendResultAsync(appResult.ToHttpResult());
        }
    }
}
