using System.Text.Json.Serialization;

namespace Core.Pagination
{
    public record PagingRequest(int PageIndex, int PageSize, bool GetAll = false) : IPagingRequest
    {
        [JsonIgnore]
        public int Skip => PageIndex > 0 ? (PageIndex - 1) * PageSize : 0;
    }
}
