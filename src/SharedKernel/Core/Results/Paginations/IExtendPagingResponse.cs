namespace Core.Result.Paginations
{
    public interface IExtendPagingResponse<T> : IPagingResponse<T>
    {
        public long PageCount { get; }
        public long Total { get; }
    }
}
