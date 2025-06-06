using System.Security.Claims;

namespace Ordering.API.Infrastruture.Services
{
    public class User(IHttpContextAccessor httpContext) : IUser, IScoped
    {
        private readonly Dictionary<string, string>? _claims = httpContext.HttpContext?.User.Claims.ToDictionary(x => x.Type, x => x.Value);

        public UserInfo Info()
        {
            var info = TryGetInfo();
            if(info == null || info.Id == Guid.Empty || string.IsNullOrEmpty(info.Email)) 
                throw new NullReferenceException(nameof(info));

            return info!;
        }

        public UserInfo? TryGetInfo()
        {
            if(_claims == null) return null;

            return new UserInfo()
            {
                Id = _claims.TryGetValue(ClaimTypes.NameIdentifier, out string? id) ? Guid.Parse(id) : Guid.Empty,
                Name = _claims.TryGetValue(ClaimTypes.Name, out string? name) ? name : null,
                Email = _claims.TryGetValue(ClaimTypes.Email, out string? email) ? email : "",
                GivenName = _claims.TryGetValue(ClaimTypes.GivenName, out string? givenName) ? givenName : null,
                SurName = _claims.TryGetValue(ClaimTypes.Surname, out string? surname) ? surname : null,
            };
        }
    }
}
