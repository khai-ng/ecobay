namespace Core.Result.Paginations
{
    public class PagingProcessor : PagingResponse<PagingProcessor>
    {
        internal PagingProcessor(IPagingRequest request) : base(request)
        {
            PageIndex = request.PageIndex;
            PageSize = request.PageSize;
        }
    }

    public static class PagingProcessorExtension
    {

        public static PagingResponse<T> Taking<T>(this PagingProcessor pagingProcessor, 
            IEnumerable<T> data)
            where T : class
        {
            return PagingResponse<T>.Taking(pagingProcessor, data);
        }

        /// <summary>
        /// Set result collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static PagingResponse<T> Result<T>(this PagingProcessor pagingProcessor, 
            IEnumerable<T> data) 
            where T : class
        {
            return PagingResponse<T>.Result(pagingProcessor, data);
        }

        /// <summary>
        /// Process paging collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static PagingResponse<T> Paging<T>(this PagingProcessor pagingProcessor, 
            IEnumerable<T> data)
            where T : class
        {
            return PagingResponse<T>.Paging(pagingProcessor, data);
        }

        /// <summary>
        /// Process paging collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>

        public static async Task<PagingResponse<T>> PagingAsync<T>(this PagingProcessor pagingProcessor, 
            IQueryable<T> data)
            where T : class
        {
            return await PagingResponse<T>.PagingAsync(pagingProcessor, data);
        }
    }
}
