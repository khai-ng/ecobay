using Core.Result;
using EmployeeManagement.Application.Services;
using EmployeeManagement.Domain.Entities;
using FastEndpoints;
using MediatR;

namespace EmployeeManagement.API.Endpoint
{
    public class GetEmployeeEndpoint : Endpoint<GetEmployeeRequest, AppResult<PagingResponse<Employee>>>
    {
        private readonly IMediator _mediator;
        public GetEmployeeEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }
        public override void Configure()
        {
            Get("Identity/GetEmployee");
        }

        public override async Task HandleAsync(GetEmployeeRequest request, CancellationToken ct)
        {
            var result = await _mediator.Send(request, ct);
            await SendResultAsync(result.ToHttpResult());
        }
    }
}
