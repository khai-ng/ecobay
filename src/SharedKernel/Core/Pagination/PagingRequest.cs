using System.Text.Json.Serialization;

namespace Core.Pagination
{
    public record PagingRequest(int PageIndex, int PageSize, bool? GetAll = false) : IPagingRequest
    {
        public static PagingRequest All()
        {
            return new(1, int.MaxValue) { GetAll = true };
        }
        public int PageIndex { get; } = PageIndex > 0 ? PageIndex :  throw new ArgumentOutOfRangeException(nameof(PageIndex));
        public int PageSize { get; } = PageSize > 0 ? PageSize :  throw new ArgumentOutOfRangeException(nameof(PageIndex));
        
        [JsonIgnore]
        public int Skip => (PageIndex - 1) * PageSize;
    }
}
