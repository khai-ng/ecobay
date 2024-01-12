namespace Core.Result
{
    public class AppResult : AppResult<AppResult>
    {
        public AppResult() { }

        protected internal AppResult(ResultStatus status)
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
            return new AppResult(ResultStatus.Error) { Errors = errorMessages };
        }

        public static AppResult Invalid(ValidationError validationError)
        {
            return new AppResult(ResultStatus.Invalid) { ValidationErrors = { validationError } };
        }

        public static AppResult Invalid(params ValidationError[] validationErrors)
        {
            return new AppResult(ResultStatus.Invalid) { ValidationErrors = new List<ValidationError>(validationErrors) };
        }

        public static AppResult Invalid(List<ValidationError> validationErrors)
        {
            return new AppResult(ResultStatus.Invalid) { ValidationErrors = validationErrors };
        }

        public new static AppResult NotFound()
        {
            return new AppResult(ResultStatus.NotFound);
        }

        public new static AppResult NotFound(params string[] errorMessages)
        {
            return new AppResult(ResultStatus.NotFound) { Errors = errorMessages };
        }

        public new static AppResult Forbidden()
        {
            return new AppResult(ResultStatus.Forbidden);
        }

        public new static AppResult Unauthorized()
        {
            return new AppResult(ResultStatus.Unauthorized);
        }
    }
}
