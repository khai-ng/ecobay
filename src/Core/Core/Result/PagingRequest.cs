using System.Text.Json.Serialization;

namespace Core.Result
{
	public class PagingRequest
	{
		public int PageIndex { get; set; }
		public int PageSize { get; set; }

		[JsonIgnore]
		public int Skip => PageIndex > 0 ? (PageIndex - 1) * PageSize : 0;
	}
}
