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
            => new(request);
        
    }

    public static class FluentPagingExtension
    {
        public static FluentPaging SetTotal(this FluentPaging fluentPaging, long total)
        {
            fluentPaging.SetTotal(total);
            return fluentPaging;
        }

        public static PagingResponse<T> Taking<T>(this FluentPaging fluentPaging, 
            IEnumerable<T> data)
            where T : class
            => PagingResponse<T>.Taking(fluentPaging, data);     

        /// <summary>
        /// Set result collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static PagingResponse<T> Result<T>(this FluentPaging fluentPaging, 
            IEnumerable<T> data) 
            where T : class
            => PagingResponse<T>.Result(fluentPaging, data);
        

        /// <summary>
        /// Process paging collection
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
