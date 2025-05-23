﻿namespace Ordering.API.Domain.OrderAggregate
{
    public class OrderItem
    {
        [MaxLength(24)]
        public string ProductId { get; private set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal Price { get; private set; }
        public int Qty { get; private set; }

        [JsonConstructor]
        private OrderItem() { }
        public OrderItem(
            string productId,
            decimal price,
            int qty)
        {
            ProductId = productId;
            Price = price;
            Qty = qty;
        }

        public void AddQty(int qty)
        {
            Qty += qty;
        }

    }
}
