using Core.AspNet.Result;
using FastEndpoints;
using Identity.Application.Services;
using MediatR;
namespace Identity.API.Presentation.Endpoint
{
    public class LoginEndpoint : Endpoint<LoginRequest, HttpResultTyped<string>>
    {
        private readonly IMediator _mediator;

        public LoginEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }
        public override void Configure()
        {
            Post("identity/login");
            AllowAnonymous();
            Summary(s =>
            {
                s.ExampleRequest = new LoginRequest("username", "password");
            });
        }

        public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
        {
            var result = await _mediator.Send(req, ct);
            await SendResultAsync(result.ToHttpResult());
        }
    }
}
