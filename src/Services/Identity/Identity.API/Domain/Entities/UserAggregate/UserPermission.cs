namespace Identity.Domain.Entities.UserAggregate
{
    public class UserPermission
    {
        public Ulid UserId { get; set; }
        public int PermissionId { get; set; }
    }
}
