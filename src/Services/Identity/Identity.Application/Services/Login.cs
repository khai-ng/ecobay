using Infrastructure.Kernel.Dependency;
using Infrastructure.Kernel.Result;
using Identity.Application.Abstractions;
using Identity.Application.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Identity.Application.Services
{
    public class Login : IRequestHandler<LoginRequest, AppResult<string>>, ITransient
    {
        private readonly IAppDbContext _context;
        private readonly IJwtProvider _jwtProvider;
        public Login(IAppDbContext context, IJwtProvider jwtProvider)
        {
            _context = context;
            _jwtProvider = jwtProvider;
        }
        public async Task<AppResult<string>> Handle(LoginRequest request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .Where(x => x.UserName == request.UserName)
                .SingleOrDefaultAsync();

            if (user == null)
                return AppResult.NotFound();

            var hashPassword = PasswordExtension.GeneratePassword(request.Password, user.SecurityStamp);

            var token = _jwtProvider.Genereate(user!);
            return user.PasswordHash.Equals(hashPassword.Password) 
                ? AppResult.Success(token) 
                : AppResult.Forbidden();
        }
    }
}