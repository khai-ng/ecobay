namespace Core.Result.Paginations
{
    public class ExtendFluentPaging : ExtendPagingResponse<ExtendFluentPaging>
    {

        internal ExtendFluentPaging(IPagingRequest request) : base(request)
        {
            PageIndex = request.PageIndex;
            PageSize = request.PageSize;
        }
    }

    public static class ExtendFluentPagingExtension
    {
        public static ExtendFluentPaging Extend(this FluentPaging fluentPaging)
        {
            return new ExtendFluentPaging(fluentPaging);
        }

        /// <summary>
        /// Set result collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ExtendPagingResponse<T> Result<T>(this ExtendFluentPaging pagingProcessor, 
            IEnumerable<T> data) 
            where T : class
        {
            return ExtendPagingResponse<T>.Result(pagingProcessor, data);
        }

        /// <summary>
        /// Process paging collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ExtendPagingResponse<T> Paging<T>(this ExtendFluentPaging pagingProcessor, 
            IEnumerable<T> data)
            where T : class
        {
            return ExtendPagingResponse<T>.Paging(pagingProcessor, data);
        }
    }
}
