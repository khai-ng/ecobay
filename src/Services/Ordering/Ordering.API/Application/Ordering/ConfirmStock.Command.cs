namespace Ordering.API.Application.Ordering
{
    public record ConfirmStockCommand(Guid OrderId) : IRequest<AppResult<string>>
    { }
}
