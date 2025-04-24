namespace Ordering.API.Application.Dto.Order
{
    public class ProductQty(string id, int qty)
    {
        public string Id { get; set; } = id;
        public int Qty { set; get; } = qty;
    }
}
