namespace Core.Pagination
{
    public interface IPagingResponse : IPagingRequest
    {
        public bool HasNext { get; }
    }

    public interface IPagingResponse<T> : IPagingResponse
    {
        public IEnumerable<T> Data { get; }
    }

    public interface ICountedPagingResponse : IPagingResponse
    {
        public long PageCount { get; }
        public long TotalCount { get; }
    }

    public interface ICountedPagingResponse<T> : IPagingResponse<T>, ICountedPagingResponse { }
}
