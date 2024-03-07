namespace Kernel.Result
{
    public class PagingTyped
    {
        public static PagingProto From(IPagingRequest request)
        {
            return new PagingProto(request);
        }
    }

    public class PagingProto : PagingResponse<PagingProto>
    {

        internal PagingProto(IPagingRequest request) :base(request)
        {
            PageIndex = request.PageIndex;
            PageSize = request.PageSize;
        }

        /// <summary>
        /// Set collection result
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public PagingResponse<T> Result<T>(IEnumerable<T> data) where T : class
        {
            return PagingResponse<T>.Result(this, data);
        }

        /// <summary>
        /// Paging collection
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
