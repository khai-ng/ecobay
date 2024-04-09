namespace Core.Result.Paginations
{
    public interface IPagingRequest
    {
        int PageIndex { get; }
        int PageSize { get; }
        public int Skip { get; }
    }

    public interface IPagingRequestExtend
    {
        int PageIndex { get; }
        int PageSize { get; }
        public int Skip { get; }
    }
}
