using Core.Autofac;
using Core.Result.AppResults;
using Core.SharedKernel;
using Identity.Application.Abstractions;
using Identity.Application.Extensions;
using Identity.Domain.Entities.UserAggregate;
using MediatR;

namespace Identity.Application.Services
{
    public class SignIn : IRequestHandler<SignInRequest, AppResult<string>>, ITransient
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        public SignIn(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<AppResult<string>> Handle(
            SignInRequest request,
            CancellationToken ct)
        {
            var pwdGen = PasswordExtension.GeneratePassword(request.Password);
            var user = new User
            {
                Id = Ulid.NewUlid(),
                UserName = request.UserName,
                Email = "",
                PasswordHash = pwdGen.Password,
                SecurityStamp = pwdGen.PasswordSalt

            };
            _userRepository.AddRange(new List<User>() { user });

            await _unitOfWork.SaveChangesAsync();
            return AppResult.Success("Sign In Sucess");
        }
    }
}
