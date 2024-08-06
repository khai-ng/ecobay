using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.API.Domain.OrderAggregate;
using Ordering.API.Domain.OrderAgrregate;

namespace Ordering.API.Infrastructure.Configurations
{
    internal sealed class OrderStatusConfiguration : IEntityTypeConfiguration<OrderStatus>
    {
        public void Configure(EntityTypeBuilder<OrderStatus> builder)
        {
            builder.HasData(OrderStatus.GetValues());
        }
    }
}
