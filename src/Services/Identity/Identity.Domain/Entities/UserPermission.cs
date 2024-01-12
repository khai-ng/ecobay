namespace Identity.Domain.Entities
{
    public class UserPermission
    {
        public Guid UserId { get; set; }
        public int PermissionId { get; set; }
    }
}
