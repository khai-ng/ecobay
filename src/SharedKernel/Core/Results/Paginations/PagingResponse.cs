namespace Core.Result.Paginations
{
    public class PagingResponse<T> : PagingRequest, IPagingResponse<T> where T : class
    {
        public IEnumerable<T> Data { get; protected set; }
        public bool HasNext { get; set; }

        public PagingResponse(IPagingRequest request)
        {
            if (request.PageSize < 1 || request.PageIndex < 1)
                throw new ArgumentOutOfRangeException();

            PageSize = request.PageSize;
            PageIndex = request.PageIndex;
        }

        public void SetData(IEnumerable<T> data) => Data = data;

        internal static PagingResponse<T> Result<TModel>(IPagingResponse<TModel> request,
            IEnumerable<T> data)
            where TModel : class
        {
            var response = new PagingResponse<T>(request)
            {
                Data = data,
                HasNext = request.HasNext,
            };
            return response;
        }

        internal static PagingResponse<T> Taking(IPagingRequest request,
            IEnumerable<T> data)
        {
            var response = new PagingResponse<T>(request);
            IEnumerable<T> filterData = data
                .Take(response.PageSize + 1);
            response.Data = filterData
                .Take(response.PageSize);
            response.HasNext = response.PageSize < filterData.Count();

            return response;
        }

        /// <summary>
        /// Paging collection
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal static PagingResponse<T> Paging(IPagingRequest request,
            IEnumerable<T> data)
        {
            var response = new PagingResponse<T>(request);

            var filterData = data
                .Skip(response.Skip)
                .Take(response.PageSize + 1);
            response.Data = filterData
                .Take(response.PageSize);
            response.HasNext = response.PageSize < filterData.Count();

            return response;
        }
    }
}
