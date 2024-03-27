namespace Core.Result
{
    public class PagingTyped
    {
        public static PagingProcessor From(IPagingRequest request)
        {
            return new PagingProcessor(request);
        }
    }

    public class PagingProcessor : PagingResponse<PagingProcessor>
    {

        internal PagingProcessor(IPagingRequest request) :base(request)
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
        public PagingResponse<T> Result<T>(IEnumerable<T> data) where T : class
        {
            return PagingResponse<T>.Result(this, data);
        }

        /// <summary>
        /// Process paging collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public PagingResponse<T> Paging<T>(IEnumerable<T> data)
            where T : class
        {
            return PagingResponse<T>.Paging(this, data);
        }
    }
}
