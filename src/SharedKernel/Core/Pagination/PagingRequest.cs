using System.Text.Json.Serialization;

namespace Core.Pagination
{
    public class PagingRequest : IPagingRequest
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        [JsonIgnore]
        public int Skip => PageIndex > 0 ? (PageIndex - 1) * PageSize : 0;
        [JsonIgnore]
        public bool GetAll { get; set; } = false;
        protected PagingRequest() { }
        public PagingRequest(int pageIndex, int pageSize, bool getAll = false)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            GetAll = getAll;
        }
    }
}
