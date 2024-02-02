namespace Kernel.Result
{
    public interface IAppResult<T>
    {
        public AppStatusCode Status { get; }
        string? Message { get; }
        IEnumerable<ErrorDetail>? Errors { get; }
        public T? Data { get; }
    }
}
