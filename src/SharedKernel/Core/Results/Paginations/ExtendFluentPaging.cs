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
            => new(fluentPaging);
        

        /// <summary>
        /// Set result collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ExtendPagingResponse<T> Result<T>(this ExtendFluentPaging extendFluentPaging, 
            IEnumerable<T> data) 
            where T : class
            => ExtendPagingResponse<T>.Result(extendFluentPaging, data);
        

        /// <summary>
        /// Process paging collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ExtendPagingResponse<T> Paging<T>(this ExtendFluentPaging extendFluentPaging, 
            IEnumerable<T> data)
            where T : class
            => ExtendPagingResponse<T>.Paging(extendFluentPaging, data);
        
    }
}
