using ProductAggregate.API.Domain.ProductAggregate;

namespace ProductAggregate.API.Application.Product
{
    public class ProductItemDto
    {
        public string Id { get; set; }
        public string MainCategory { get; set; }
        public string Title { get; set; }
        public decimal AverageRating { get; set; }
        public decimal RatingNumber { get; set; }
        public string? Price { get; set; }
        public IEnumerable<Image>? Images { get; set; }
        public IEnumerable<Video>? Videos { get; set; }
        public string? Store { get; set; }
        public IEnumerable<string>? Categories { get; set; }
        public object? Details { get; set; }
    }
}
