//using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace SharedKernel.Kernel.Result
{
	public class AppResult<T> : IAppResult
    {
        public T Value { get; }
        public ResultStatus Status { get; set; }

        [JsonIgnore]
        public bool IsSuccess => Status == ResultStatus.Ok;

        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Message { get; protected set; } = string.Empty;

        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IEnumerable<string> Errors { get; protected set; } = new List<string>();

        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<ValidationError> ValidationErrors { get; protected set; } = new List<ValidationError>();

        protected AppResult() { }

        public AppResult(T value)
        {
            Value = value;
        }

        protected AppResult(ResultStatus status)
        {
            Status = status;
        }

        public static implicit operator T(AppResult<T> result)
        {
            return result.Value;
        }

        public static implicit operator AppResult<T>(T value)
        {
            return new AppResult<T>(value);
        }

        public static implicit operator AppResult<T>(AppResult result) => new AppResult<T>(default(T))
        {
            Status = result.Status,
            Errors = result.Errors,
        };

        public static AppResult<T> Success(T value)
        {
            return new AppResult<T>(value);
        }

        public static AppResult<T> Error(params string[] errorMessages)
        {
            return new AppResult<T>(ResultStatus.Error) { Errors = errorMessages };
        }

        public static AppResult<T> Invalid(ValidationError validationError)
        {
            return new AppResult<T>(ResultStatus.Invalid) { ValidationErrors = { validationError } };
        }
        
        public static AppResult<T> Invalid(params ValidationError[] validationErrors)
        {
            return new AppResult<T>(ResultStatus.Invalid) { ValidationErrors = new List<ValidationError>(validationErrors) };
        }

        public static AppResult<T> Invalid(List<ValidationError> validationErrors)
        {
            return new AppResult<T>(ResultStatus.Invalid) { ValidationErrors = validationErrors };
        }

        public static AppResult<T> NotFound()
        {
            return new AppResult<T>(ResultStatus.NotFound);
        }

        public static AppResult<T> NotFound(params string[] errorMessages)
        {
            return new AppResult<T>(ResultStatus.NotFound) { Errors = errorMessages };
        }

        public static AppResult<T> Forbidden()
        {
            return new AppResult<T>(ResultStatus.Forbidden);
        }

        public static AppResult<T> Unauthorized()
        {
            return new AppResult<T>(ResultStatus.Unauthorized);
        }
    }
}
