using SharedKernel.Kernel.Result;
using MediatR;

namespace Identity.Application.Services
{
    public record LoginRequest(string UserName, string Password) : IRequest<AppResult<string>>
    { }
}
