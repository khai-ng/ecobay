using ProductAggregate.API.Infrastructure.Shared;

namespace ProductAggregate.API.Application.Dto.Product
{
    public class ProductUnit(string id, int units) : IHashable
    {
        public string Id { get; set; } = id;
        public int Units { set; get; } = units;
    }
}
