namespace Ordering.API.Domain.OrderAggregate
{
    public class OrderItem: Entity
    {
        [ForeignKey(nameof(Order))]
        public Guid OrderId { get; private set; }
        [MaxLength(24)]
        public string ProductId { get; private set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal UnitPrice { get; private set; }
        public int Unit { get; private set; }

        private OrderItem() { }
        public OrderItem(
            string productId,
            decimal unitPrice,
            int unit)
        {
            ProductId = productId;
            UnitPrice = unitPrice;
            Unit = unit;
        }

        public void AddUnits(int units)
        {
            Unit += units;
        }

    }
}
