using Core.Result.Paginations;
using MongoDB.Driver;

namespace Core.MongoDB.Paginations
{
    public static class ExtendPagingResponseExtension
    {
        public static async Task<ExtendPagingResponse<T>> PagingAsync<T>(IPagingRequest request,
            IFindFluent<T, T> data)
            where T : class
        {
            var rs = await PagingResponseExtension.PagingAsync(request, data);

            var response = new ExtendPagingResponse<T>(rs);
            response.Total = await data.CountDocumentsAsync();
            response.PageCount = (long)Math.Ceiling((decimal)response.Total / response.PageSize);

            return response;
        }

        public static IQueryable<TEntity> Filter<TEntity>(this ExtendPagingResponse<TEntity> response,
            IQueryable<TEntity> data)
            where TEntity : class
        {
            response.Total = data.LongCount();
            response.PageCount = (long)Math.Ceiling((decimal)response.Total / response.PageSize);

            return response.Filter(data);
        }
    }
}
