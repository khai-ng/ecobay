using ProductAggregate.API.Application.Hashing;

namespace ProductAggregate.API.Application.Product
{
    public class ProductUnitDto(string id, int units) : IHashable
    {
        public string Id { get; set; } = id;
        public int Units { set; get; } = units;
    }
}
