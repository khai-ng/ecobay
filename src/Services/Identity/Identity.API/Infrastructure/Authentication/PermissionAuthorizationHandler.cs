using Identity.Application.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.IdentityModel.Tokens.Jwt;

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
            if (!Ulid.TryParse(userId, out Ulid parsedUserId))
                return;


            var pendingRequirements = context.PendingRequirements.ToList();
            List<RolesAuthorizationRequirement> roleRequirements = new();

            foreach (var item in pendingRequirements)
            {
                if (item is RolesAuthorizationRequirement itemRole)
                    roleRequirements.Add(itemRole);
            }

            using IServiceScope serviceScope = _serviceScopeFactory.CreateScope();
            var userService = serviceScope.ServiceProvider.GetRequiredService<IUserRepository>();
            var roles = await userService.GetListRoleAsync(parsedUserId);


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
            if(!Ulid.TryParse(userId, out Ulid pasredUserId))
                return;

            using IServiceScope serviceScope = _serviceScopeFactory.CreateScope();
            var userService = serviceScope.ServiceProvider.GetRequiredService<IUserRepository>();
            var claims = await userService.GetListPermissionAsync(pasredUserId);

            if(claims.Contains(requirement.Permission))
                context.Succeed(requirement);
        }
    }
}
