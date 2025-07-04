using System.Text.Json.Serialization;

namespace Core.Pagination
{
    public record PagingRequest(int PageIndex, int PageSize) : IPagingRequest
    {
        public int PageIndex { get; } = PageIndex > 0 ? PageIndex :  throw new ArgumentOutOfRangeException(nameof(PageIndex));
        public int PageSize { get; } = PageSize > 0 ? PageSize :  throw new ArgumentOutOfRangeException(nameof(PageIndex));
        
        [JsonIgnore]
        public int Skip => (PageIndex - 1) * PageSize;
    }

    public record AllablePagingRequest(int PageIndex, int PageSize, bool? GetAll) : IAllablePagingRequest
    {
        public int PageIndex { get; } = PageIndex > 0 ? PageIndex : throw new ArgumentOutOfRangeException(nameof(PageIndex));
        public int PageSize { get; } = PageSize > 0 ? PageSize : throw new ArgumentOutOfRangeException(nameof(PageIndex));


        [JsonIgnore]
        public int Skip => (PageIndex - 1) * PageSize;
    }
}
