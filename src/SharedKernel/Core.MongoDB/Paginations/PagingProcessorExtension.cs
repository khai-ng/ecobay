using Core.Result.Paginations;
using MongoDB.Driver;

namespace Core.MongoDB.Paginations
{
    public static class PagingProcessorExtension
    {
        /// <summary>
        /// Process paging collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>

        public static async Task<PagingResponse<TOut>> PagingAsync<TIn, TOut>(this FluentPaging fluentPaging,
            IFindFluent<TIn, TOut> data)
            where TIn : class
            where TOut: class
        {
            return await PagingResponseExtension.PagingAsync(fluentPaging, data);
        }
    }
}
