using Core.Result;
using EmployeeMgt.Domain.Entities;
using MediatR;

namespace EmployeeMgt.Application.Services
{
    public class GetEmployeeRequest: PagingRequest, IRequest<AppResult<PagingResponse<Employee>>>
    {
        public string? EmployeeName { get; set; }
    }
}
