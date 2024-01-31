using SharedKernel.Kernel.Enum;

namespace Identity.Domain.Entities
{
    public class Role : Enumeration<Role>
    {

        public static readonly Role Admin = new(1, nameof(Admin));
        public static readonly Role Labor = new(2, nameof(Labor));
        public static readonly Role Training = new(3, nameof(Training));
        public Role(int id, string name) : base(id, name) { }

        public ICollection<User> Users { get; set; }
        public ICollection<Permission> Permissions { get; set; }
    }
}
