using Core.Autofac;
using Core.IntegrationEvents;
using Core.Result.AppResults;
using Identity.Application.Abstractions;
using Identity.Application.Extensions;
using MediatR;

namespace Identity.Application.Services
{
    public class Login : IRequestHandler<LoginRequest, AppResult<string>>, ITransient
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtProvider _jwtProvider;
        private readonly IIntegrationProducer _producer;
        public Login(IUserRepository userRepository, IJwtProvider jwtProvider, IIntegrationProducer producer)
        {
            _userRepository = userRepository;
            _jwtProvider = jwtProvider;
            _producer = producer;
        }
        public async Task<AppResult<string>> Handle(
            LoginRequest request, 
            CancellationToken ct)
        {
            var user = await _userRepository.FindAsync(request.UserName);

            if (user == null)
                return AppResult.NotFound("User name or password not match!");

            var hashPassword = PasswordExtension.GeneratePassword(request.Password, user.SecurityStamp);
            if (!user.PasswordHash.Equals(hashPassword.Password))
                return AppResult.Forbidden();

            var token = _jwtProvider.Genereate(user!);
            //await _producer.PublishAsync(new HelloEvent("Hello employee, i'm identity"));

            return AppResult.Success(token);
        }
    }
}