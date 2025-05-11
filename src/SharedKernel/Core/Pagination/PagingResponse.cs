using System.Text.Json.Serialization;

namespace Core.Pagination
{
    public class PagingResponse<T> : IPagingRequest, IPagingResponse<T> where T : class
    {
        public IEnumerable<T> Data { get; private set; }
        public bool HasNext { get; private set; }

        public int PageIndex { get; private set; }

        public int PageSize { get; private set; }

        [JsonIgnore]
        public int Skip => PageIndex > 0 ? (PageIndex - 1) * PageSize : 0;
        [JsonIgnore]
        public bool? GetAll { get; } = false;

        public PagingResponse(IPagingRequest request) => Initialized(request);

        public PagingResponse(IAllablePagingRequest request) 
        {
            GetAll = request.GetAll ?? false;
            Initialized(request);
        }

        private void Initialized(IPagingRequest request)
        {
            if (GetAll ?? false)
            {
                PageIndex = 1;
                PageSize = int.MaxValue;
                return;
            }
            if (request.PageSize < 1 || request.PageIndex < 1)
                throw new ArgumentOutOfRangeException();

            PageSize = request.PageSize;
            PageIndex = request.PageIndex;
        }

        public PagingResponse<T> SetData(IEnumerable<T> data)
        {
            Data = data;
            return this;
        }

        public PagingResponse<T> SetHasNext(bool hasNext)
        {
            HasNext = hasNext;
            return this;
        }

        internal static PagingResponse<T> Result<TModel>(
            IPagingResponse<TModel> request,
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

        /// <summary>
        /// Paging collection
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal static PagingResponse<T> Paging(
            IAllablePagingRequest request,
            IEnumerable<T> data)
        {
            var response = new PagingResponse<T>(request);

            if (!response.GetAll ?? false)
                data = data.Skip(response.Skip)
                    .Take(response.PageSize + 1);

            response.Data = data
                .Take(response.PageSize);
            response.HasNext = response.PageSize < data.Count();

            return response;
        }
    }
}
