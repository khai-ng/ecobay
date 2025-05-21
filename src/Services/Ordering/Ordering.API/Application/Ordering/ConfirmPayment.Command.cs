namespace Ordering.API.Application.Ordering
{
    public record ConfirmPaymentCommand(Guid OrderId) : IRequest<AppResult<string>>
    { }
}
