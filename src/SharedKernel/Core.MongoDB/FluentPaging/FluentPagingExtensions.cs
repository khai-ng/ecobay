using Core.Pagination;
using MongoDB.Driver;

namespace Core.MongoDB.Paginations
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
            
        public static Task<PagingResponse<TOut>> PagingAsync<TIn, TOut>(this FluentPaging request, IFindFluent<TIn, TOut> data)
            where TIn : class
            where TOut : class
            => PagingExtensions.PagingAsync(request, data);

        /// <summary>
        /// Filter data collection base on <see cref="PagingRequest"/> and apply it into result
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="paging"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static IFindFluent<TIn, TOut> FilterApply<TIn, TOut>(this FluentPaging paging, IFindFluent<TIn, TOut> data)
            where TIn : class
            where TOut : class
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
        public static Task<CountedPagingResponse<TOut>> PagingAsync<TIn, TOut>(this CountedFluentPaging request, IFindFluent<TIn, TOut> data)
            where TIn : class
            where TOut : class
            => CountedPagingExtensions.PagingAsync(request, data);

        /// <summary>
        /// Filter data collection base on <see cref="PagingRequest"/> and apply it into result
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="paging"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static IFindFluent<TIn, TOut> FilterApply<TIn, TOut>(this CountedFluentPaging paging, IFindFluent<TIn, TOut> data)
            where TIn : class
            where TOut : class
            => CountedPagingExtensions.FilterApply(paging, data);
    }
}
