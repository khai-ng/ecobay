namespace Identity.Domain.Entities.UserAggrigate
{
    public class UserPermission
    {
        public Guid UserId { get; set; }
        public int PermissionId { get; set; }
    }
}
