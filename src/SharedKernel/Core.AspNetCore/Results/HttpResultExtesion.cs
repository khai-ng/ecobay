using Core.Result.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace Core.AspNet.Result
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
        {
            return new HttpResult<T>(appResult);
        }

        public static async Task<T?> ToValueAsync<T>(this IResult result)
        {
            var mockHttpContext = new DefaultHttpContext
            {
                RequestServices = new ServiceCollection().AddLogging().BuildServiceProvider(),
                Response = { Body = new MemoryStream() }
            };
            await result.ExecuteAsync(mockHttpContext);

            // Reset MemoryStream to start so we can read the response.
            mockHttpContext.Response.Body.Position = 0;

            var jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
            return await JsonSerializer.DeserializeAsync<T>(mockHttpContext.Response.Body, jsonOptions);
        }
    }
}
