using System.Text.Json.Serialization;

namespace Core.Result
{
    public class PagingRequest : IPagingRequest
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        [JsonIgnore]
        public int Skip => PageIndex > 0 ? (PageIndex - 1) * PageSize : 0;

        protected PagingRequest() { }
    }

    public interface IPagingRequest 
	{
		int PageIndex { get; }
		int PageSize { get; }
        public int Skip { get; }
    }
}
