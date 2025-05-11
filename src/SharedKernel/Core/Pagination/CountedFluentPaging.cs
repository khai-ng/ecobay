namespace Core.Pagination
{
    public class CountedFluentPaging : CountedPagingResponse<CountedFluentPaging>
    {

        internal CountedFluentPaging(IPagingRequest request) : base(request) { }
        internal CountedFluentPaging(IAllablePagingRequest request) : base(request) { }

        public static CountedFluentPaging From(IPagingRequest request) => new(request);
        public static CountedFluentPaging From(IAllablePagingRequest request) => new(request);
    }

    public static class CountedFluentPagingExtension
    {
        /// <summary>
        /// Enrich counting into result. Or using <see cref="CountedFluentPaging"/> instead
        /// </summary>
        /// <param name="fluentPaging"></param>
        /// <returns></returns>
        public static CountedFluentPaging Counted(this FluentPaging fluentPaging)
            => new(fluentPaging);

        /// <summary>
        /// Set total into result
        /// </summary>
        /// <param name="fluentPaging"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public static CountedFluentPaging SetTotal(this CountedFluentPaging fluentPaging, long total)
        {
            fluentPaging.SetTotal(total);
            return fluentPaging;
        }

        /// <summary>
        /// Set data collection into response without paging
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static CountedPagingResponse<T> Result<T>(this CountedFluentPaging extendFluentPaging,
            IEnumerable<T> data)
            where T : class
            => CountedPagingResponse<T>.Result(extendFluentPaging, data);


        /// <summary>
        /// Process paging data collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static CountedPagingResponse<T> Paging<T>(this CountedFluentPaging extendFluentPaging,
            IEnumerable<T> data)
            where T : class
            => CountedPagingResponse<T>.Paging(extendFluentPaging, data);

    }
}
