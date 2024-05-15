using EmployeeMgt.Application.Abstractions;
using EmployeeMgt.Domain.Entities;
using MediatR;
using Core.Autofac;
using Core.Result.Paginations;
using Core.Result.AppResults;

namespace EmployeeMgt.Application.Services
{
    public class GetEmployee : IRequestHandler<GetEmployeeRequest, AppResult<PagingResponse<Employee>>>, ITransient
    {
        private readonly IAppDbContext _context;

        public GetEmployee(IAppDbContext context)
        {
            _context = context;
        }

        public Task<AppResult<PagingResponse<Employee>>> Handle(
            GetEmployeeRequest request, 
            CancellationToken ct)
        {
            var masterData = _context.Employees
                .Where(x => string.IsNullOrEmpty(request.EmployeeName)
                    || x.Name.Contains(request.EmployeeName));

            var pagingProcessor = PagingTyped.From(request);
            var pagedData = pagingProcessor.Filter(masterData);
            var pageEmployee = pagingProcessor.Result(pagedData);

            return Task.FromResult(AppResult.Success(pageEmployee));
        }
    }
}
