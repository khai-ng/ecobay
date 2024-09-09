using Core.Result.AppResults;

namespace Core.Result.Abstractions
{
    public interface IAppResult<T> : IAppResult
    {
        public T? Data { get; }
    }

    public interface IAppResult
    {
        public bool IsSuccess { get; }
        public AppStatusCode Status { get; }
        string? Message { get; }
        IEnumerable<ErrorDetail>? Errors { get; }
    }
}
