namespace Core.Pagination
{
    public class CountedPagingResponse<T> : PagingResponse<T>, ICountedPagingResponse<T>
        where T : class
    {
        public long PageCount { get; protected set; }

        public long TotalCount { get; protected set; }

        public CountedPagingResponse(IPagingRequest request) : base(request)
        { }

        public CountedPagingResponse(IPagingResponse<T> request) : base(request)
        {
            HasNext = request.HasNext;
            Data = request.Data;
        }

        public CountedPagingResponse<T> Total(long total)
        {
            TotalCount = total;
            PageCount = this.GetAll ? 1 : (long)Math.Ceiling((decimal)TotalCount / PageSize);
            return this;
        }

        internal static CountedPagingResponse<T> Result<TModel>(ICountedPagingResponse<TModel> request,
            IEnumerable<T> data)
            where TModel : class
        {
            var rs = PagingResponse<T>.Result(request, data);
            var response = new CountedPagingResponse<T>(rs)
            {
                PageCount = request.PageCount,
                TotalCount = request.TotalCount,
            };

            return response;
        }

        internal static new CountedPagingResponse<T> Paging(IPagingRequest request,
            IEnumerable<T> data)
        {
            var rs = PagingResponse<T>.Paging(request, data);

            var response = new CountedPagingResponse<T>(rs);
            response.Total(data.LongCount());

            return response;
        }
    }
}