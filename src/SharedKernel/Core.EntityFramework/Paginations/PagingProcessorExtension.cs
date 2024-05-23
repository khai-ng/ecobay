using Core.Result.Paginations;

namespace Core.EntityFramework.Paginations
{
    public static class PagingProcessorExtension
    {
        /// <summary>
        /// Process paging collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>

        public static async Task<PagingResponse<T>> PagingAsync<T>(this FluentPaging fluentPaging,
            IQueryable<T> data)
            where T : class
        {
            return await PagingResponseExtension.PagingAsync(fluentPaging, data);
        }
    }
}
