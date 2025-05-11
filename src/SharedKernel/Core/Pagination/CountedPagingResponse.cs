namespace Core.Pagination
{
    public class CountedPagingResponse<T> : PagingResponse<T>, ICountedPagingResponse<T>
        where T : class
    {
        public long PageCount { get; private set; }

        public long TotalCount { get; private set; }

        public CountedPagingResponse(IPagingRequest request) : base(request) { }
        public CountedPagingResponse(IAllablePagingRequest request) : base(request) { }

        public CountedPagingResponse(IPagingResponse<T> request) : base(request)
        {
            SetHasNext(request.HasNext);
            SetData(request.Data);
        }

        public CountedPagingResponse<T> SetTotal(long total)
        {
            TotalCount = total;
            PageCount = this.GetAll ?? false ? 1 : (long)Math.Ceiling((decimal)TotalCount / PageSize);
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

        internal static new CountedPagingResponse<T> Paging(IAllablePagingRequest request,
            IEnumerable<T> data)
        {
            var rs = PagingResponse<T>.Paging(request, data);

            var response = new CountedPagingResponse<T>(rs);
            response.SetTotal(data.LongCount());

            return response;
        }
    }
}