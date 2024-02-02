namespace Kernel.Result
{
    public class AppResult : AppResult<AppResult>
    {
        public AppResult() { }

        protected internal AppResult(AppStatusCode status)
        : base(status)
        { }

        public static AppResult Success()
        {
            return new AppResult();
        }

        public static AppResult<T> Success<T>(T value)
        {
            return new AppResult<T>(value);
        }

        public new static AppResult Error(params string[] errorMessages)
        {
            return new AppResult(AppStatusCode.Error) { Errors = errorMessages.Select(e => new ErrorDetail(e)) };
        }

        public static new AppResult Invalid(ErrorDetail error)
        {
            return new AppResult(AppStatusCode.Invalid) { Errors = new List<ErrorDetail>() { error } };
        }

        public static new AppResult Invalid(params ErrorDetail[] errors)
        {
            return new AppResult(AppStatusCode.Invalid) { Errors = new List<ErrorDetail>(errors) };
        }

        public static new AppResult Invalid(IEnumerable<ErrorDetail> errors)
        {
            return new AppResult(AppStatusCode.Invalid) { Errors = errors };
        }

        public new static AppResult NotFound()
        {
            return new AppResult(AppStatusCode.NotFound);
        }

        public new static AppResult NotFound(params string[] errorMessages)
        {
            return new AppResult(AppStatusCode.NotFound) { Errors = errorMessages.Select(e => new ErrorDetail(e)) };
        }

        public new static AppResult Forbidden()
        {
            return new AppResult(AppStatusCode.Forbidden);
        }

        public new static AppResult Unauthorized()
        {
            return new AppResult(AppStatusCode.Unauthorized);
        }
    }
}
