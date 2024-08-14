namespace Identity.Domain.Entities.UserAggregate
{
    public class UserRole
    {
        public Guid UserId { get; set; }
        public int RoleId { get; set; }
    }
}
