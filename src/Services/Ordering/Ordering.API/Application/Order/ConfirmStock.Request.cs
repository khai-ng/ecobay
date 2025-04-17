using Core.AppResults;
using MediatR;

namespace Ordering.API.Application.Services
{
    public class ConfirmStockRequest : IRequest<AppResult<string>>
    {
        public Guid OrderId { get; set; }
    }
}
