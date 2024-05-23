using Core.Result.Paginations;
using MongoDB.Driver;

namespace Core.MongoDB.Paginations
{
    public static class ExtendPagingProcessorExtension
    {
        /// <summary>
        /// Process paging collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static async Task<ExtendPagingResponse<T>> PagingAsync<T>(this ExtendFluentPaging fluentPaging,
            IFindFluent<T, T> data)
            where T : class
        {
            return await ExtendPagingResponseExtension.PagingAsync(fluentPaging, data);
        }
    }
}
