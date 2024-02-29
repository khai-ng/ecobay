//using Identity.Application.Abstractions;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Identity;
//using SharedKernel.Kernel.Dependency;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;

//namespace Identity.Infrastructure.Authentication
//{
//    public class AppUser : IAppUser, IScoped
//    {
//        private readonly IHttpContextAccessor _httpContext;
//        private readonly ClaimsPrincipal _claimsPrincipal;

//        public AppUser(IHttpContextAccessor httpContext)
//        {
//            _httpContext = httpContext;
//            _claimsPrincipal = _httpContext.HttpContext?.User ?? new ClaimsPrincipal();
//        }

//        public Guid? Id => Guid.Parse(IdentityUser.Id);

//        public string? UserName => IdentityUser.UserName;

//        public string? Domain => throw new NotImplementedException();

//        private IdentityUser IdentityUser 
//        {
//            get 
//            {
//                return new IdentityUser()
//                {
//                    Id = _claimsPrincipal.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value ?? "",
//                    UserName = _claimsPrincipal.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email)?.Value
//                };
//            } 
//        }
//    }
//}
