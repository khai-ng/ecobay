using System.Net;

namespace Core.Result.AppResults
{
    public class AppResult : AppResult<AppResult>
    {
        public AppResult() { }

        protected internal AppResult(AppStatusCode status)
        : base(status)
        { }

        /// <summary>
        /// Creates result with status <see cref="AppStatusCode.Ok"/>, equivalent <seealso cref="HttpStatusCode.OK"/><br/>
        /// Indicates that the request has succeeded
        /// </summary>
        /// <returns></returns>
        public static AppResult Success()
        {
            return new AppResult();
        }

        /// <summary>
        /// Creates result with status <see cref="AppStatusCode.Ok"/>, equivalent <seealso cref="HttpStatusCode.OK"/><br/> 
        /// Indicates that the request has succeeded
        /// </summary>
        /// <returns></returns>
        public static AppResult<T> Success<T>(T value)
        {
            return new AppResult<T>(value);
        }

        /// <summary>
        /// Creates result with status <see cref="AppStatusCode.Error"/>, equivalent <seealso cref="HttpStatusCode.InternalServerError"/><br/>
        /// Indicates that the server has occur unexpected error
        /// </summary>
        /// <returns></returns>
        public new static AppResult Error(string message)
        {
            return new AppResult(AppStatusCode.Error) { Errors = new List<ErrorDetail> { new ErrorDetail(message) } };
        }

        /// <summary>
        /// Creates result with status <see cref="AppStatusCode.Error"/>, equivalent <seealso cref="HttpStatusCode.InternalServerError"/><br/>
        /// Indicates that the server has occur unexpected error
        /// </summary>
        /// <returns></returns>
        public new static AppResult Error(params string[] errorDetailMessages)
        {
            return new AppResult(AppStatusCode.Error) { Errors = errorDetailMessages.Select(e => new ErrorDetail(e)) };
        }

        /// <summary>
        /// Creates result with status <see cref="AppStatusCode.Invalid"/>, equivalent <seealso cref="HttpStatusCode.BadRequest"/><br/>
        /// Indicates that the request could not be understood by the server
        /// </summary>
        /// <returns></returns>
        public static new AppResult Invalid(ErrorDetail error)
        {
            return new AppResult(AppStatusCode.Invalid) { Errors = new List<ErrorDetail>() { error } };
        }

        /// <summary>
        /// Creates result with status <see cref="AppStatusCode.Invalid"/>, equivalent <seealso cref="HttpStatusCode.BadRequest"/><br/>
        /// Indicates that the request could not be understood by the server
        /// </summary>
        /// <returns></returns>
        public static new AppResult Invalid(params ErrorDetail[] errors)
        {
            return new AppResult(AppStatusCode.Invalid) { Errors = new List<ErrorDetail>(errors) };
        }

        /// <summary>
        /// Creates result with status <see cref="AppStatusCode.Invalid"/>, equivalent <seealso cref="HttpStatusCode.BadRequest"/><br/>
        /// Indicates that the request could not be understood by the server
        /// </summary>
        /// <returns></returns>
        public static new AppResult Invalid(IEnumerable<ErrorDetail> errors)
        {
            return new AppResult(AppStatusCode.Invalid) { Errors = errors };
        }

        /// <summary>
        /// Creates result with status <see cref="AppStatusCode.NotFound"/>, equivalent <seealso cref="HttpStatusCode.NotFound"/><br/>
        /// Indicates that the server did not find a current representation for the target resource
        /// </summary>
        /// <returns></returns>
        public new static AppResult NotFound()
        {
            return new AppResult(AppStatusCode.NotFound);
        }

        /// <summary>
        /// Creates result with status <see cref="AppStatusCode.NotFound"/>, equivalent <seealso cref="HttpStatusCode.NotFound"/><br/>
        /// Indicates that the server did not find a current representation for the target resource
        /// </summary>
        /// <returns></returns>
        public new static AppResult NotFound(string message)
        {
            return new AppResult(AppStatusCode.NotFound) { Message = message };
        }

        /// <summary>
        /// Creates result with status <see cref="AppStatusCode.NotFound"/>, equivalent <seealso cref="HttpStatusCode.NotFound"/><br/>
        /// Indicates that the server did not find a current representation for the target resource
        /// </summary>
        /// <returns></returns>
        public new static AppResult NotFound(params string[] errorDetailMessages)
        {
            return new AppResult(AppStatusCode.NotFound) { Errors = errorDetailMessages.Select(e => new ErrorDetail(e)) };
        }

        /// <summary>
        /// Creates result with status <see cref="AppStatusCode.Forbidden"/>, equivalent <seealso cref="HttpStatusCode.Forbidden"/><br/>
        /// Indicates that the server understood the request but refuses to authorize
        /// </summary>
        /// <returns></returns>
        public new static AppResult Forbidden()
        {
            return new AppResult(AppStatusCode.Forbidden);
        }

        /// <summary>
        /// Creates result with status <see cref="AppStatusCode.Unauthorized"/>, equivalent <seealso cref="HttpStatusCode.Unauthorized"/><br/>
        /// Indicates that the request requires authorization
        /// </summary>
        /// <returns></returns>
        public new static AppResult Unauthorized()
        {
            return new AppResult(AppStatusCode.Unauthorized);
        }
    }
}
