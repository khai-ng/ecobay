using Ordering.API.Infrastruture.Services;

namespace Ordering.API.Application.Abstractions
{
    public interface IUser
    {
        /// <summary>
        /// Get user info
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        UserInfo Info();

        UserInfo? TryGetInfo();
    }
}
