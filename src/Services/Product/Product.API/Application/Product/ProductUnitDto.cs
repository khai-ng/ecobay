namespace Product.API.Application.Product
{
    public class ProductUnitDto(string id, int units)
    {
        public string Id { get; set; } = id;
        public int Units { set; get; } = units;
    }
}
