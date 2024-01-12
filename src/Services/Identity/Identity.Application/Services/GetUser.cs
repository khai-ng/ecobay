using Core.Dependency;
using Core.Result;
using Identity.Application.Abstractions;
using Identity.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Identity.Application.Services
{
	public class GetUser : IRequestHandler<GetUserRequest, AppResult<PagingResponse<User>>>, IScoped
    {
        private readonly IAppDbContext _context;
        public GetUser(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<AppResult<PagingResponse<User>>> Handle(GetUserRequest request, CancellationToken cancellationToken)
        {

            var pageUser = PagingTyped.PagingResult(await _context.Users.ToListAsync(), request);

            return AppResult.Success(pageUser);
		}
    }
}