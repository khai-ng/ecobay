using System.Linq;
using System.Security.Claims;

namespace Ordering.API.Infrastruture.Services
{
    public class User(IHttpContextAccessor httpContext) : IUser, IScoped
    {
        private readonly IEnumerable<Claim>? _claims = httpContext.HttpContext?.User.Claims;

        public UserInfo Info()
        {
            var info = TryGetInfo();
            if(info == null || info.Id == Guid.Empty || string.IsNullOrEmpty(info.Email)) 
                throw new NullReferenceException(nameof(info));

            return info!;
        }

        public UserInfo? TryGetInfo()
        {
            if (_claims == null) return null;

            static string? GetClaimValue(IEnumerable<Claim> claims, string type)
                => claims.FirstOrDefault(c => c.Type == type)?.Value;

            return new UserInfo()
            {
                Id = Guid.TryParse(GetClaimValue(_claims, ClaimTypes.NameIdentifier), out Guid parsedId) ? parsedId : Guid.Empty,
                Name = GetClaimValue(_claims, ClaimTypes.Name),
                Email = GetClaimValue(_claims, ClaimTypes.Email) ?? string.Empty,
                GivenName = GetClaimValue(_claims, ClaimTypes.GivenName),
                SurName = GetClaimValue(_claims, ClaimTypes.Surname),
            };
        }
    }
}
