using System.Text.Json.Serialization;

namespace Core.Pagination
{
    public record PagingRequest(int PageIndex, int PageSize) : IPagingRequest
    {
        [JsonIgnore]
        public int Skip => PageIndex > 0 ? (PageIndex - 1) * PageSize : 0;
    }

    public record AllablePagingRequest(int PageIndex, int PageSize, bool? GetAll) : IAllablePagingRequest
    {
        [JsonIgnore]
        public int Skip => PageIndex > 0 ? (PageIndex - 1) * PageSize : 0;
    }
}
