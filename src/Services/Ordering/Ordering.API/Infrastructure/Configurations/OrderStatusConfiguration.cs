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
