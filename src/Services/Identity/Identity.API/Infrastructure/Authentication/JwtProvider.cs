using Core.AspNet.Extensions;
using Core.Autofac;
using Identity.API.Application.Common.Abstractions;
using Identity.API.Application.Common.Constants;
using Identity.Domain.Entities.UserAggregate;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Identity.Infrastructure.Authentication
{
    internal class JwtProvider : IJwtProvider, IScoped
    {
        private IConfiguration _configuration;
        public JwtProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Genereate(User user)
        {
            var jwtOptions = _configuration.GetSection(AppEnvironment.JWT_SECTION).Get<JwtOption>();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions!.Key));
            var credential = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new System.Security.Claims.Claim[]
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Email, user.UserName)
            };

            var token = new JwtSecurityToken(
                issuer: jwtOptions!.Issuer,
                audience: jwtOptions!.Audience,
                claims,
                expires: DateTime.UtcNow.AddMinutes(120),
                signingCredentials: credential);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}