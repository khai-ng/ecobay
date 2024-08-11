using FastEndpoints;
using Ordering.API.Application.Services;

namespace Ordering.API.Presentation.Endpoint
{
    public class AddOrderValidator : Validator<CreateOrderRequest>
    {
        public AddOrderValidator() 
        {
        }
    }
}
