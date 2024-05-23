using Core.AspNet.Result;
using FastEndpoints;
using Identity.Application.Services;
using MediatR;
namespace Identity.API.Endpoint
{
    public class SignInEndpoint : Endpoint<SignInRequest, HttpResultTyped<string>>
    {
        private readonly IMediator _mediator;

        public SignInEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }
        public override void Configure()
        {
            Post("identity/signin");
            AllowAnonymous();
            Summary(s =>
            {
                s.ExampleRequest = new SignInRequest("username", "password");
            });
        }

        public override async Task HandleAsync(SignInRequest req, CancellationToken ct)
        {
            var result = await _mediator.Send(req, ct);
            await SendResultAsync(result.ToHttpResult());
        }
    }
}
