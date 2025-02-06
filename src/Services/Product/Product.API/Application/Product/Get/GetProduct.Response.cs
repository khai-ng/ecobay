namespace Product.API.Application.Product.Get
{
    public class GetProductItem
    {
        public string Id { get; set; }
        public string MainCategory { get; set; }
        public string Title { get; set; }
        public decimal AverageRating { get; set; }
        public decimal RatingNumber { get; set; }
        public string? Price { get; set; }
        public string? Image { get; set; }
        public string? Store { get; set; }
    }
}
