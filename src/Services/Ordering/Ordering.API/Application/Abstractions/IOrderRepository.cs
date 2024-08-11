using Core.EntityFramework.Repository;
using Ordering.API.Domain.OrderAgrregate;

namespace Ordering.API.Application.Abstractions
{
    public interface IOrderRepository: IRepository<Order>
    {
    }
}
