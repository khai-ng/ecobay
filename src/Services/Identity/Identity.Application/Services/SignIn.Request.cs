using Core.Result;
using MediatR;

namespace Identity.Application.Services
{
    public record SignInRequest(string UserName, string Password) : IRequest<AppResult<string>>
    { }
}
