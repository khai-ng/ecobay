using Identity.Domain.Entities.UserAggregate;

namespace Identity.Application.Abstractions
{
    public interface IJwtProvider
    {
        string Genereate(User user);
    }
}