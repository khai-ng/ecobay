namespace Core.Pagination
{
    public interface IPagingRequest
    {
        int PageIndex { get; }
        int PageSize { get; }
        public int Skip { get; }
    }

    public interface IAllablePagingRequest : IPagingRequest
    {
        bool? GetAll { get; }
    }
}
