using Microsoft.AspNetCore.Authorization;

namespace Identity.Infrastructure.Authentication
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public PermissionRequirement(string permission) 
        {
            Permission = permission;
        }

        public string Permission { get; set; }
    }
}
