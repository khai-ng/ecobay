using Microsoft.AspNetCore.Http;

namespace Kernel.Result
{

    public static class HttpResultExtesion 
    {
        /// <summary>
        /// Convert IAppResult into Http.IResult
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="appResult"></param>
        /// <exception cref="NotSupportedException"></exception>
        /// <returns>IResult</returns>
        public static IResult ToHttpResult<T>(this IAppResult<T> appResult)
            where T : class
        {
            return new HttpResult<T>(appResult);
        }
    }
}
