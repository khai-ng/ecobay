using Core.ServiceDefault;
using Core.SharedKernel;

namespace Identity.Domain.Entities.UserAggrigate
{
    public class User : BaseAggregateRoot
    {
        public required string UserName { get; set; }
        public string Name { get; set; } = string.Empty;
        public required string Email { get; set; }
        public string Phone { get; set; } = string.Empty;
        public required string PasswordHash { get; set; }
        public required string SecurityStamp { get; set; }

        public ICollection<Role> Roles { get; set; }
        public ICollection<Permission> Permissions { get; set; }
    }
}
