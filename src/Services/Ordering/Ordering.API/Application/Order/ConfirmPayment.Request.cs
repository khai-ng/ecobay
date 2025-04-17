using Core.AppResults;
using MediatR;

namespace Ordering.API.Application.Services
{
    public class ConfirmPaymentRequest: IRequest<AppResult<string>>
    {
        public Guid OrderId { get; set; }
    }
}
