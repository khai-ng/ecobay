using Identity.Domain.Entities;

namespace Identity.Application.Abstractions
{
    public interface IJwtProvider
    {
        string Genereate(User user);
    }
}