namespace Core.Result.Paginations
{
    public interface IPagingResponseExtend<T> : IPagingResponse<T>
    {
        public long PageCount { get; }
        public long Total { get; }
    }
}
