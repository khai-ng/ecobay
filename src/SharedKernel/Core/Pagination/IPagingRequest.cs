namespace Core.Pagination
{
    public interface IPagingRequest
    {
        int PageIndex { get; }
        int PageSize { get; }
        bool? GetAll { get; }
        public int Skip { get; }
    }
}
