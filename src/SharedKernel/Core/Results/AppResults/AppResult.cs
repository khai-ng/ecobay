using Core.Result.Abstractions;

namespace Core.Result.AppResults
{
    public class AppResult<T> : IAppResult<T>
    {
        public AppStatusCode Status { get; set; }
        public T? Data { get; set; }
        public string? Message { get; protected set; }
        public IEnumerable<ErrorDetail>? Errors { get; protected set; }

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

        public static implicit operator AppResult<T>(AppResult result) => new(default(T))
        {
            Status = result.Status,
            Errors = result.Errors,
        };

        /// <summary>
        /// Creates result with status <see cref="AppStatusCode.Ok"/>, equivalent <seealso cref="HttpStatusCode.OK"/><br/>
        /// Indicates that the request has succeeded
        /// </summary>
        /// <returns></returns>

        public static AppResult<T> Success(T value)
        {
            return new AppResult<T>(value);
        }

        /// <summary>
        /// Creates result with status <see cref="AppStatusCode.Error"/>, equivalent <seealso cref="HttpStatusCode.InternalServerError"/><br/>
        /// Indicates that the server has occur unexpected error
        /// </summary>
        /// <returns></returns>
        public static AppResult<T> Error(string message)
        {
            return new AppResult<T>(AppStatusCode.Error) { Message = message };
        }

        /// <summary>
        /// Creates result with status <see cref="AppStatusCode.Error"/>, equivalent <seealso cref="HttpStatusCode.InternalServerError"/><br/>
        /// Indicates that the server has occur unexpected error
        /// </summary>
        /// <returns></returns>
        public static AppResult<T> Error(params string[] errorDetailMessages)
        {
            return new AppResult<T>(AppStatusCode.Error) { Errors = errorDetailMessages.Select(e => new ErrorDetail(e)) };
        }

        /// <summary>
        /// Creates result with status <see cref="AppStatusCode.Invalid"/>, equivalent <seealso cref="HttpStatusCode.BadRequest"/><br/>
        /// Indicates that the request could not be understood by the server
        /// </summary>
        /// <returns></returns>
        public static AppResult<T> Invalid(ErrorDetail error)
        {
            return new AppResult<T>(AppStatusCode.Invalid) { Errors = new List<ErrorDetail>() { error } };
        }

        /// <summary>
        /// Creates result with status <see cref="AppStatusCode.Invalid"/>, equivalent <seealso cref="HttpStatusCode.BadRequest"/><br/>
        /// Indicates that the request could not be understood by the server
        /// </summary>
        /// <returns></returns>
        public static AppResult<T> Invalid(params ErrorDetail[] errors)
        {
            return new AppResult<T>(AppStatusCode.Invalid) { Errors = errors };
        }

        /// <summary>
        /// Creates result with status <see cref="AppStatusCode.Invalid"/>, equivalent <seealso cref="HttpStatusCode.BadRequest"/><br/>
        /// Indicates that the request could not be understood by the server
        /// </summary>
        /// <returns></returns>
        public static AppResult<T> Invalid(IEnumerable<ErrorDetail> errors)
        {
            return new AppResult<T>(AppStatusCode.Invalid) { Errors = errors };
        }

        /// <summary>
        /// Creates result with status <see cref="AppStatusCode.NotFound"/>, equivalent <seealso cref="HttpStatusCode.NotFound"/><br/>
        /// Indicates that the server did not find a current representation for the target resource
        /// </summary>
        /// <returns></returns>
        public static AppResult<T> NotFound()
        {
            return new AppResult<T>(AppStatusCode.NotFound);
        }

        /// <summary>
        /// Creates result with status <see cref="AppStatusCode.NotFound"/>, equivalent <seealso cref="HttpStatusCode.NotFound"/><br/>
        /// Indicates that the server did not find a current representation for the target resource
        /// </summary>
        /// <returns></returns>
        public static AppResult<T> NotFound(string message)
        {
            return new AppResult<T>(AppStatusCode.NotFound) { Message = message };
        }

        /// <summary>
        /// Creates result with status <see cref="AppStatusCode.NotFound"/>, equivalent <seealso cref="HttpStatusCode.NotFound"/><br/>
        /// Indicates that the server did not find a current representation for the target resource
        /// </summary>
        /// <returns></returns>
        public static AppResult<T> NotFound(params string[] errorDetailMessages)
        {
            return new AppResult<T>(AppStatusCode.NotFound) { Errors = errorDetailMessages.Select(e => new ErrorDetail(e)) };
        }

        /// <summary>
        /// Creates result with status <see cref="AppStatusCode.Forbidden"/>, equivalent <seealso cref="HttpStatusCode.Forbidden"/><br/>
        /// Indicates that the server understood the request but refuses to authorize
        /// </summary>
        /// <returns></returns>
        public static AppResult<T> Forbidden()
        {
            return new AppResult<T>(AppStatusCode.Forbidden);
        }

        /// <summary>
        /// Creates result with status <see cref="AppStatusCode.Unauthorized"/>, equivalent <seealso cref="HttpStatusCode.Unauthorized"/><br/>
        /// Indicates that the request requires authorization
        /// </summary>
        /// <returns></returns>
        public static AppResult<T> Unauthorized()
        {
            return new AppResult<T>(AppStatusCode.Unauthorized);
        }
    }
}
