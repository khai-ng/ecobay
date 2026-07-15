namespace Ordering.API.Domain.OrderAggregate
{
    public class OrderItem
    {
        [MaxLength(24)]
        public string ProductId { get; private set; }
        public string ProductName { get; private set; }
        public string ImageUrl { get; private set; }
        public decimal Price { get; private set; }
        public int Qty { get; private set; }

        public OrderItem(
            string productId,
            string productName,
            string imageUrl,
            decimal price,
            int qty)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(price);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(qty);

            ProductId = productId;
            ProductName = productName;
            ImageUrl = imageUrl;
            Price = price;
            Qty = qty;
        }

        public void AddQty(int qty)
        {
            Qty += qty;
        }

    }
}
