using Core.Result;
using EmployeeManagement.Domain.Entities;
using MediatR;

namespace EmployeeManagement.Application.Services
{
    public class GetEmployeeRequest: PagingRequest, IRequest<AppResult<PagingResponse<Employee>>>
    {
        public string? EmployeeName { get; set; }
    }
}
