namespace Ordering.API.Application.Dto.Order
{
    public class ProductUnit(string id, int units)
    {
        public string Id { get; set; } = id;
        public int Units { set; get; } = units;
    }
}
