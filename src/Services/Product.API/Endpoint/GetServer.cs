using Core.AspNet.Result;
using Core.Result.AppResults;
using FastEndpoints;
using MediatR;
using Product.API.Application.Servers;
using Product.API.Domain.ServerAggregate;

namespace Product.API.Endpoint
{
    public class GetServerEndpoint: EndpointWithoutRequest<HttpResultTyped<AppResult<IEnumerable<Server>>>> //Endpoint<GetServerRequest, HttpResultTyped<AppResult<IEnumerable<Server>>>>
    {
        private readonly IMediator _mediator;
        public GetServerEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }
        public override void Configure()
        {
            Get("server/get");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var request = new GetServerRequest();
            var result = await _mediator.Send(request, ct);
            await SendResultAsync(result.ToHttpResult());
        }
    }
}
