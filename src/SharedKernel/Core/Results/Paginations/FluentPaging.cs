namespace Core.Result.Paginations
{
    public class FluentPaging : PagingResponse<FluentPaging>
    {
        internal FluentPaging(IPagingRequest request) : base(request)
        {
            PageIndex = request.PageIndex;
            PageSize = request.PageSize;
        }
        public static FluentPaging From(IPagingRequest request)
        {
            return new FluentPaging(request);
        }
    }

    public static class FluentPagingExtension
    {

        public static PagingResponse<T> Taking<T>(this FluentPaging fluentPaging, 
            IEnumerable<T> data)
            where T : class
        {
            return PagingResponse<T>.Taking(fluentPaging, data);
        }

        /// <summary>
        /// Set result collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static PagingResponse<T> Result<T>(this FluentPaging fluentPaging, 
            IEnumerable<T> data) 
            where T : class
        {
            return PagingResponse<T>.Result(fluentPaging, data);
        }

        /// <summary>
        /// Process paging collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static PagingResponse<T> Paging<T>(this FluentPaging fluentPaging, 
            IEnumerable<T> data)
            where T : class
        {
            return PagingResponse<T>.Paging(fluentPaging, data);
        }
    }
}
