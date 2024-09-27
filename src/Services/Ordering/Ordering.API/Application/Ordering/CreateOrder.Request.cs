using Core.AppResults;
using MediatR;

namespace Ordering.API.Application.Services
{
    public class CreateOrderRequest: IRequest<AppResult<Guid>>
    {
        public Guid BuyerId { get; set; }
        public Guid PaymentId { get; set; }
        public string Country { get; set; }

        public string City { get; set; }
        public string District { get; set; }
        public string Street { get; set; }

        public List<OrderItemRequest> OrderItems { get; set; }

        public CreateOrderRequest(Guid buyerId,
            Guid paymentId,
            string country,
            string city,
            string district,
            string street,
            List<OrderItemRequest> orderItems) 
        {
            BuyerId = buyerId;
            PaymentId = paymentId;
            Country = country;
            City = city;
            District = district;
            Street = street;
            OrderItems = orderItems;
        }
    }

    public class OrderItemRequest
    {
        public string ProductId { get; set; }
        public decimal UnitPrice { get; set; }
        public int Unit { get; set; }

        /// <summary>
        /// Exception:
        /// <see cref="ArgumentOutOfRangeException"/>
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="unitPrice"></param>
        /// <param name="unit"></param>
        public OrderItemRequest(string productId,
            decimal unitPrice,
            int unit)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(unit, 1);

            ProductId = productId;
            UnitPrice = unitPrice;
            Unit = unit;
        }
    }
}
