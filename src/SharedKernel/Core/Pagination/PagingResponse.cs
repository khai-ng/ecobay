using System.Text.Json.Serialization;

namespace Core.Pagination
{
    public class PagingResponse<T> : IPagingRequest, IPagingResponse<T> where T : class
    {
        public IEnumerable<T> Data { get; protected set; }
        public bool HasNext { get; set; }

        public int PageIndex { get; protected set; }

        public int PageSize { get; protected set; }

        [JsonIgnore]
        public int Skip => PageIndex > 0 ? (PageIndex - 1) * PageSize : 0;
        [JsonIgnore]
        public bool GetAll { get; protected set; }

        public PagingResponse(IPagingRequest request)
        {
            GetAll = request.GetAll;
            if (GetAll)
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

        /// <summary>
        /// Paging collection
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal static PagingResponse<T> Paging(IPagingRequest request,
            IEnumerable<T> data)
        {
            var response = new PagingResponse<T>(request);

            if (!request.GetAll)
                data = data.Skip(response.Skip)
                    .Take(response.PageSize + 1);

            response.Data = data
                .Take(response.PageSize);
            response.HasNext = response.PageSize < data.Count();

            return response;
        }
    }
}
