namespace Ordering.API.Application.Services
{
    public record ConfirmStockCommand(Guid OrderId) : IRequest<AppResult<string>>
    { }
}
