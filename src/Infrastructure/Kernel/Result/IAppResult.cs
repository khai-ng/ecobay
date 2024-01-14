namespace Infrastructure.Kernel.Result
{
    public interface IAppResult
    {
        string Message { get; }
        ResultStatus Status { get; }
        IEnumerable<string> Errors { get; }
    }
}
