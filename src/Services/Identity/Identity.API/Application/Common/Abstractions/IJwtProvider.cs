using Identity.Domain.Entities.UserAggregate;

namespace Identity.API.Application.Common.Abstractions
{
    public interface IJwtProvider
    {
        string Genereate(User user);
    }
}