using FastEndpoints;
using Identity.Application.Services;
using MediatR;
using Kernel.Result;
namespace Identity.API.Endpoint
{
    public class LoginEndpoint : Endpoint<LoginRequest, IResult>
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
