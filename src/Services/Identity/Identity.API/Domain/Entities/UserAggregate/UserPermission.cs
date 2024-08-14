namespace Identity.Domain.Entities.UserAggregate
{
    public class UserPermission
    {
        public Guid UserId { get; set; }
        public int PermissionId { get; set; }
    }
}
