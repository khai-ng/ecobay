using System.Text.Json.Serialization;

namespace Core.Result.AppResults
{
    public class BaseResponse
    {
        public int StatusCode { get; set; }
        public string Title { get; set; } = string.Empty;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Message { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IEnumerable<ErrorDetail>? Errors { get; set; }
    }
}
