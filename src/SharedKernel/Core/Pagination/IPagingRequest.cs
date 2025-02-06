namespace Core.Pagination
{
    public interface IPagingRequest
    {
        int PageIndex { get; }
        int PageSize { get; }
        public int Skip { get; }
        /// <summary>
        /// Marked to skip paging and get all data
        /// </summary>
        bool GetAll { get; }
    }
}
