using FastEndpoints;
using Web.ApiGateway.Services;

namespace Web.ApiGateway.Endpoint
{
    public class EmployeeEndpoint : EndpointWithoutRequest<string>
    {
        private readonly IEmployeeService _employeeService;
        public EmployeeEndpoint(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        //public EmployeeEndpoint() { }
        public override void Configure()
        {
            Get("employee/hello");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var rs = await _employeeService.SayHelloAsync();
            await SendResultAsync(Results.Ok(rs));
            //await SendResultAsync(Results.Ok());
        }
    }
}
