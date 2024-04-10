using Identity.Domain.Entities.UserAggrigate;

namespace Identity.Application.Abstractions
{
    public interface IJwtProvider
    {
        string Genereate(User user);
    }
}