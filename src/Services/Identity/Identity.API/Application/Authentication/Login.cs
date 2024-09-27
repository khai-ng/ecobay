using Core.Autofac;
using Core.IntegrationEvents.IntegrationEvents;
using Core.AppResults;
using Identity.API.Application.Common.Abstractions;
using Identity.API.Application.Common.Extensions;
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
            //await _producer.PublishAsync(new HelloEvent("Hello employee, i'm identity"));

            var user = await _userRepository.FindAsync(request.UserName).ConfigureAwait(false);

            if (user == null)
                return AppResult.NotFound("User name or password not match!");

            var hashPassword = PasswordExtension.GeneratePassword(request.Password, user.SecurityStamp);
            if (!user.PasswordHash.Equals(hashPassword.Password))
                return AppResult.Forbidden();

            var token = _jwtProvider.Genereate(user!);

            return AppResult.Success(token);
        }
    }

    public class HelloEvent: IntegrationEvent
    {
        public HelloEvent(string message) { Message = message; }
        public string Message { get; set; } = string.Empty;
    }
}