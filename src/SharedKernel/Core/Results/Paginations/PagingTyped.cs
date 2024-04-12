namespace Core.Result.Paginations
{
    public class PagingTyped
    {
        public static PagingProcessor From(IPagingRequest request)
        {
            return new PagingProcessor(request);
        }
    }
}
