namespace Core.Result.Paginations
{
    public class ExtendPagingResponse<T> : PagingResponse<T>, IExtendPagingResponse<T> 
        where T : class
    {
        public long PageCount { get; protected set; }

        public long Total { get; protected set; }

        public ExtendPagingResponse(IPagingRequest request) : base(request)
        {
        }

        public ExtendPagingResponse(IPagingResponse<T> request) : base(request)
        {
            HasNext = request.HasNext;
            Data = request.Data;
        }

        public void SetTotal(long total)
        {
            Total = total;
            PageCount = (long)Math.Ceiling((decimal)Total / PageSize);
        }

        internal static ExtendPagingResponse<T> Result<TModel>(IExtendPagingResponse<TModel> request, 
            IEnumerable<T> data)
            where TModel : class
        {
            var rs = PagingResponse<T>.Result(request, data);
            var response = new ExtendPagingResponse<T>(rs)
            {
                PageCount = request.PageCount,
                Total = request.Total,
            };

            return response;
        }

        internal static new ExtendPagingResponse<T> Paging(IPagingRequest request, 
            IEnumerable<T> data)
        {
            var rs = PagingResponse<T>.Paging(request, data);

            var response = new ExtendPagingResponse<T>(rs);
            response.SetTotal(data.LongCount());

            return response;
        }
    }
}