namespace Product.API.Application.Product
{
    public class ProductQtyDto(string id, int qty)
    {
        public string Id { get; set; } = id;
        public int Qty { set; get; } = qty;
    }
}
