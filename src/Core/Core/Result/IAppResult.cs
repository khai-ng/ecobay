namespace Core.Result
{
    public interface IAppResult
    {
        string Message { get; }
        ResultStatus Status { get; }
        IEnumerable<string> Errors { get; }
    }
}
