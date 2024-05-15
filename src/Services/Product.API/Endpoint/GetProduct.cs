using Core.Result.AppResults;
using Core.Result.Paginations;
using FastEndpoints;
using MediatR;
using Product.API.Application;
using Core.AspNet.Result;

namespace Product.API.Endpoint
{
    public class GetProductEndpoint: Endpoint<GetProductRequest, AppResult<PagingResponse<Domain.Product>>>
    {
        private readonly IMediator _mediator;
        public GetProductEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }
        public override void Configure()
        {
            Get("product/get");
            AllowAnonymous();
        }

        public override async Task HandleAsync(GetProductRequest request, CancellationToken ct)
        {
            var result = await _mediator.Send(request, ct);
            await SendResultAsync(result.ToHttpResult());
        }
    }
}
