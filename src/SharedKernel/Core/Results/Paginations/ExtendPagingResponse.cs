namespace Core.Result.Paginations
{
    public class ExtendPagingResponse<T> : PagingResponse<T>, IExtendPagingResponse<T> 
        where T : class
    {
        public long PageCount { get; set; }

        public long Total { get; set; }

        public ExtendPagingResponse(IPagingRequest request) : base(request)
        {
            PageSize = request.PageSize;
            PageIndex = request.PageIndex;
        }

        internal ExtendPagingResponse(IPagingResponse<T> request) : base(request)
        {
            PageSize = request.PageSize;
            PageIndex = request.PageIndex;
            HasNext = request.HasNext;
            Data = request.Data;
        }

        internal static ExtendPagingResponse<T> Result<TProto>(IExtendPagingResponse<TProto> request, 
            IEnumerable<T> data)
            where TProto : class
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
            response.Total = data.LongCount();
            response.PageCount = (long)Math.Ceiling((decimal)response.Total / response.PageSize);

            return response;
        }
    }
}