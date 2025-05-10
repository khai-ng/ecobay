using Core.AppResults;
using MediatR;

namespace Ordering.API.Application.Services
{
    public record ConfirmPaymentCommand(Guid OrderId) : IRequest<AppResult<string>>
    { }
}
