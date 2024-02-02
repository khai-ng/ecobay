using Kernel.Result;
using MediatR;
using EmployeeManagement.Domain.Entities;

namespace EmployeeManagement.Application.Services
{
    public class GetEmployeeRequest: PagingRequest, IRequest<AppResult<PagingResponse<Employee>>>
    {
        public string? EmployeeName { get; set; }
    }
}
