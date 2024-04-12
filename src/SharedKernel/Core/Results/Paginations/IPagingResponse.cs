namespace Core.Result.Paginations
{
    public interface IPagingResponse<T> : IPagingRequest
    {
        public IEnumerable<T> Data { get; }
        public bool HasNext { get; }
    }
}
