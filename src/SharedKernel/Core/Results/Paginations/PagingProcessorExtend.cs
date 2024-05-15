namespace Core.Result.Paginations
{
    public class PagingProcessorExtend : PagingResponseExtend<PagingProcessorExtend>
    {

        internal PagingProcessorExtend(IPagingRequest request) : base(request)
        {
            PageIndex = request.PageIndex;
            PageSize = request.PageSize;
        }
    }

    public static class PagingProcessorExtendExtension
    {
        public static PagingProcessorExtend Extend(this PagingProcessor pagingProcessor)
        {
            return new PagingProcessorExtend(pagingProcessor);
        }

        /// <summary>
        /// Set result collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static PagingResponseExtend<T> Result<T>(this PagingProcessorExtend pagingProcessor, 
            IEnumerable<T> data) 
            where T : class
        {
            return PagingResponseExtend<T>.Result(pagingProcessor, data);
        }

        /// <summary>
        /// Process paging collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static PagingResponseExtend<T> Paging<T>(this PagingProcessorExtend pagingProcessor, 
            IEnumerable<T> data)
            where T : class
        {
            return PagingResponseExtend<T>.Paging(pagingProcessor, data);
        }

        public static async Task<PagingResponseExtend<T>> PagingAsync<T>(this PagingProcessorExtend pagingProcessor, 
            IQueryable<T> data)
            where T : class
        {
            return await PagingResponseExtend<T>.PagingAsync(pagingProcessor, data);
        }
    }
}
