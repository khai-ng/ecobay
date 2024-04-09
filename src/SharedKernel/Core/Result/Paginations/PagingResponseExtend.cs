namespace Core.Result.Paginations
{
    public class PagingResponseExtend<T> : PagingResponse<T>, IPagingResponseExtend<T> where T : class
    {
        public long PageCount { get; internal set; }

        public long Total { get; internal set; }

        internal PagingResponseExtend(IPagingRequest request) : base(request)
        {
            PageSize = request.PageSize;
            PageIndex = request.PageIndex;
        }

        internal PagingResponseExtend(IPagingResponse<T> request) : base(request)
        {
            PageSize = request.PageSize;
            PageIndex = request.PageIndex;
            HasNext = request.HasNext;
            Data = request.Data;
        }

        internal static PagingResponseExtend<T> Result<TProto>(IPagingResponseExtend<TProto> request, IEnumerable<T> data)
            where TProto : class
        {
            var rs = PagingResponse<T>.Result(request, data);
            var response = new PagingResponseExtend<T>(rs)
            {
                PageCount = request.PageCount,
                Total = request.Total,
            };

            if (data.Count() > response.PageSize)
                throw new ArgumentException();

            return response;
        }

        internal static new PagingResponseExtend<T> Paging(IPagingRequest request, IEnumerable<T> data)
        {
            var rs = PagingResponse<T>.Paging(request, data);

            var response = new PagingResponseExtend<T>(rs);
            response.Total = data.LongCount();
            response.PageCount = (long)Math.Ceiling((decimal)response.Total / response.PageSize);

            return response;
        }

        internal static new async Task<PagingResponseExtend<T>> PagingAsync(IPagingRequest request, IQueryable<T> data)
        {
            var rs = await PagingResponse<T>.PagingAsync(request, data);

            var response = new PagingResponseExtend<T>(rs);
            response.Total = data.LongCount();
            response.PageCount = (long)Math.Ceiling((decimal)response.Total / response.PageSize);

            return response;
        }

        public new IQueryable<TEntity> Filter<TEntity>(IQueryable<TEntity> data)
            where TEntity : class
        {
            Total = data.LongCount();
            PageCount = (long)Math.Ceiling((decimal)Total / PageSize);

            return base.Filter(data);
        }
    }
}