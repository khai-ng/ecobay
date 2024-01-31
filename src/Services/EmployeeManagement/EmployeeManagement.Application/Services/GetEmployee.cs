using SharedKernel.Kernel.Dependency;
using SharedKernel.Kernel.Result;
using EmployeeManagement.Application.Abstractions;
using EmployeeManagement.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Application.Services
{
    public class GetEmployee : IRequestHandler<GetEmployeeRequest, AppResult<PagingResponse<Employee>>>, ITransient
    {
        private readonly IAppDbContext _context;

        public GetEmployee(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<AppResult<PagingResponse<Employee>>> Handle(GetEmployeeRequest request, CancellationToken cancellationToken)
        {
            var pageResponse = PagingTyped.Load(request);
            var filterData = _context.Employees
                .Where(x => string.IsNullOrEmpty(request.EmployeeName)
                    || x.Name.Contains(request.EmployeeName));
            var pagedData = pageResponse.PagingMaster(filterData);
            var pageEmployee = pageResponse.SetPagedData(await pagedData.ToListAsync());

            return AppResult.Success(pageEmployee);
        }
    }
}
