using System.Text.Json.Serialization;

namespace Core.AppResults
{
    public class ErrorDetail
    {
        public ErrorDetail(string message)
        {
            Message = message;
        }

        public ErrorDetail(string name, string message)
        {
            Name = name;
            Message = message;
        }

        public ErrorDetail(string name, string message, ValidationSeverity severity)
        {
            Name = name;
            Message = message;
            Severity = severity;
        }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Name { get; set; }
        public string Message { get; set; }

        [JsonIgnore]
        public ValidationSeverity Severity { get; set; } = ValidationSeverity.Error;
    }
}
