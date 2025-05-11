namespace Core.Pagination
{
    public class FluentPaging : PagingResponse<FluentPaging>
    {
        internal FluentPaging(IPagingRequest request) : base(request) { }
        internal FluentPaging(IAllablePagingRequest request) : base(request) { }

        public static FluentPaging From(IPagingRequest request) => new(request);
        public static FluentPaging From(IAllablePagingRequest request) => new(request);

    }

    public static class FluentPagingExtension
    {
        /// <summary>
        /// Set data collection into response without paging
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static PagingResponse<T> Result<T>(this FluentPaging fluentPaging,
            IEnumerable<T> data)
            where T : class
            => PagingResponse<T>.Result(fluentPaging, data);


        /// <summary>
        /// Process paging data collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static PagingResponse<T> Paging<T>(this FluentPaging fluentPaging,
            IEnumerable<T> data)
            where T : class
            => PagingResponse<T>.Paging(fluentPaging, data);

    }
}
