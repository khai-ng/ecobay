namespace Ordering.API.Infrastruture.Services
{
    public class UserInfo
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? GivenName { get; set; }
        public string? SurName { get; set; }
        public string Email { get; set; }
    }
}
