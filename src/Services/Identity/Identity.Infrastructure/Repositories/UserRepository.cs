using Core.Autofac;
using Core.Result.Paginations;
using Core.SharedKernel;
using Identity.Application.Abstractions;
using Identity.Application.Services;
using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository, IScoped
    {
        private readonly AppDbContext _context;
        public IUnitOfWork UnitOfWork => _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PagingResponse<User>> GetUsersPaging(GetUserRequest request)
        {
            return await PagingTyped
                .From(request)
                .PagingAsync(_context.Users);
        }
    }
}
