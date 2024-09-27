using Core.Pagination;

namespace Core.EntityFramework.Pagination
{
    public static class FluentPagingExtensions
    {
        /// <summary>
        /// Process paging data collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Task<PagingResponse<T>> PagingAsync<T>(this FluentPaging request, IQueryable<T> data)
            where T : class
            => PagingExtensions.PagingAsync(request, data);

        /// <summary>
        /// Filter data collection base on <see cref="PagingRequest"/> and apply it into result
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="paging"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static IQueryable<T> FilterApply<T>(this FluentPaging paging, IQueryable<T> data)
            where T : class
            => PagingExtensions.FilterApply(paging, data);
    }

    public static class CountedFluentPagingExtensions
    {
        /// <summary>
        /// Process paging data collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Task<CountedPagingResponse<T>> PagingAsync<T>(this CountedFluentPaging request, IQueryable<T> data)
            where T : class
            => CountedPagingExtensions.PagingAsync(request, data);

        /// <summary>
        /// Filter data collection base on <see cref="PagingRequest"/> and apply it into result
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="paging"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static IQueryable<T> FilterApply<T>(this CountedFluentPaging paging, IQueryable<T> data)
            where T : class
            => CountedPagingExtensions.FilterApply(paging, data);
    }
}
