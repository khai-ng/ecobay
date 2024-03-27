using Core.Autofac;
using GrpcEmployee;

namespace Web.ApiGateway.Services
{
    public interface IEmployeeService
    {
        Task<string> SayHelloAsync();
    }
    public class EmployeeService : IEmployeeService, ITransient
    {
        private readonly Employee.EmployeeClient _employeeClient;

        public EmployeeService(Employee.EmployeeClient employeeClient)
        {
            _employeeClient = employeeClient;
        }

        public async Task<string> SayHelloAsync()
        {
            var response = await _employeeClient.SayHelloAsync(new HelloRequest() { Name = "Khai" });

            return response.Message;
        }
    }
}
