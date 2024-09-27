namespace Core.Pagination
{
    public interface IPagingRequest
    {
        int PageIndex { get; }
        int PageSize { get; }
        public int Skip { get; }
        bool GetAll { get; }
    }
}
