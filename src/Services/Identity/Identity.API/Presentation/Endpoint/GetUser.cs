using Core.AspNet.Result;
using Core.Pagination;
using Core.AppResults;
using FastEndpoints;
using Identity.Application.Services;
using Identity.Domain.Entities.UserAggregate;
using MediatR;

namespace Identity.API.Presentation.Endpoint
{

    public class GetUserEndPoint : Endpoint<GetUserRequest, HttpResultTyped<PagingResponse<User>>>
    {
        private readonly IMediator _mediator;
        public GetUserEndPoint(IMediator mediator)
        {
            _mediator = mediator;
        }
        public override void Configure()
        {
            Get("identity/getuser");
            Roles(Role.Admin.Name);
            //AllowAnonymous();
        }

        public override async Task HandleAsync(GetUserRequest request, CancellationToken ct)
        {
            var result = await _mediator.Send(request, ct);
            await SendResultAsync(result.ToHttpResult());
        }
    }
}
