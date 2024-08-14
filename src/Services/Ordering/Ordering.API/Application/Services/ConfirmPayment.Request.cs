using Core.Result.AppResults;
using MediatR;

namespace Ordering.API.Application.Services
{
    public class ConfirmPaymentRequest: IRequest<AppResult>
    {
        public Guid OrderId { get; set; }
    }
}
