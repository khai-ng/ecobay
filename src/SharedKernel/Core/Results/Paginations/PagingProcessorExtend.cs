namespace Core.Result.Paginations
{
    public class PagingProcessorExtend : PagingResponseExtend<PagingProcessorExtend>
    {

        internal PagingProcessorExtend(IPagingRequest request) : base(request)
        {
            PageIndex = request.PageIndex;
            PageSize = request.PageSize;
        }

        /// <summary>
        /// Set result collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public PagingResponseExtend<T> Result<T>(IEnumerable<T> data) where T : class
        {
            return PagingResponseExtend<T>.Result(this, data);
        }

        /// <summary>
        /// Process paging collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public PagingResponseExtend<T> Paging<T>(IEnumerable<T> data)
            where T : class
        {
            return PagingResponseExtend<T>.Paging(this, data);
        }

        public async Task<PagingResponseExtend<T>> PagingAsync<T>(IQueryable<T> data)
            where T : class
        {
            return await PagingResponseExtend<T>.PagingAsync(this, data);
        }
    }

    public static class PagingProcessorExtendExtension
    {
        public static PagingProcessorExtend Extend(this PagingProcessor pagingProcessor)
        {
            return new PagingProcessorExtend(pagingProcessor);
        }
    }
}
