using Grpc.Core;

namespace GrpcEmployee
{
    public class EmployeeService : Employee.EmployeeBase
    {
        public EmployeeService() { }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply
            {
                Message = "Hello " + request.Name
            });
        }
    }
}
