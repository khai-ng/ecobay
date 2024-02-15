namespace Kernel.Result
{
    public class AppResult<T> : IAppResult<T>
    {
        public AppStatusCode Status { get; set; }
        public T? Data { get; set; }
        public string? Message { get; protected set; }
        public IEnumerable<ErrorDetail>? Errors { get; protected set; }
        public bool IsSuccess => Status == AppStatusCode.Ok;

        protected AppResult() { }

        public AppResult(T data)
        {
            Data = data;
        }

        protected AppResult(AppStatusCode status)
        {
            Status = status;
        }

        public static implicit operator T?(AppResult<T> result)
        {
            return result.Data;
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

        public static AppResult<T> Error(params string[] errors)
        {
            return new AppResult<T>(AppStatusCode.Error) { Errors = errors.Select(e => new ErrorDetail(e)) };
        }

        public static AppResult<T> Invalid(ErrorDetail error)
        {
            return new AppResult<T>(AppStatusCode.Invalid) { Errors = new List<ErrorDetail>() { error } };
        }
        
        public static AppResult<T> Invalid(params ErrorDetail[] errors)
        {
            return new AppResult<T>(AppStatusCode.Invalid) { Errors = errors };
        }

        public static AppResult<T> Invalid(IEnumerable<ErrorDetail> errors)
        {
            return new AppResult<T>(AppStatusCode.Invalid) { Errors = errors };
        }

        public static AppResult<T> NotFound()
        {
            return new AppResult<T>(AppStatusCode.NotFound);
        }

        public static AppResult<T> NotFound(params string[] errorMessages)
        {
            return new AppResult<T>(AppStatusCode.NotFound) { Errors = errorMessages.Select(e => new ErrorDetail(e)) };
        }

        public static AppResult<T> Forbidden()
        {
            return new AppResult<T>(AppStatusCode.Forbidden);
        }

        public static AppResult<T> Unauthorized()
        {
            return new AppResult<T>(AppStatusCode.Unauthorized);
        }
    }
}
