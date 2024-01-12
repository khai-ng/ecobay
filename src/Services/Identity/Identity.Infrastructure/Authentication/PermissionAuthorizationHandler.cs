using Identity.Application.Abstractions;
using Identity.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace Identity.Infrastructure.Authentication
{
    public class PermissionAuthorizationHandler: AuthorizationHandler<PermissionRequirement>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public PermissionAuthorizationHandler(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public override async Task HandleAsync(AuthorizationHandlerContext context)
        {
            var userId = context.User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value;
            if (!Guid.TryParse(userId, out Guid pasredUserId))
                return;


            var pedningRequirements = context.PendingRequirements.ToList();
            List<RolesAuthorizationRequirement> roleRequirements = new();

            foreach (var item in pedningRequirements)
            {
                if (item is RolesAuthorizationRequirement itemRole)
                    roleRequirements.Add(itemRole);
            }

            using IServiceScope serviceScope = _serviceScopeFactory.CreateScope();
            IPermissionService permissionService = serviceScope.ServiceProvider.GetRequiredService<IPermissionService>();
            var roles = await permissionService.GetRolesAsync(pasredUserId);


            foreach (var roleRequirement in roleRequirements)
            {
                foreach (var role in roles)
                {
                    if (roleRequirement.AllowedRoles.Contains(role))
                    {
                        context.Succeed(roleRequirement);
                    }
                }
            }
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var userId = context.User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value;
            if(!Guid.TryParse(userId, out Guid pasredUserId))
                return;

            using IServiceScope serviceScope = _serviceScopeFactory.CreateScope();
            IPermissionService claimService = serviceScope.ServiceProvider.GetRequiredService<IPermissionService>();
            var claims = await claimService.GetPermissionsAsync(pasredUserId);

            if(claims.Contains(requirement.Permission))
                context.Succeed(requirement);
        }
    }
}
