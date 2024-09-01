using Core.EntityFramework.ServiceDefault;

namespace Seller.API.Domain.SellerAggregate
{
    public class SellerItem: AggregateRoot
    {
        public string Name { get; set; }
    }
}
