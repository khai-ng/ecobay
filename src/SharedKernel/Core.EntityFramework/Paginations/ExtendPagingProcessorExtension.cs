using Core.Result.Paginations;

namespace Core.EntityFramework.Paginations
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
            IQueryable<T> data)
            where T : class
        {
            return await ExtendPagingResponseExtension.PagingAsync(fluentPaging, data);
        }
    }
}
