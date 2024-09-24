using Core.Result.Paginations;
using Microsoft.EntityFrameworkCore;

namespace Core.EntityFramework.Paginations
{
    public static class ExtendPagingExtensions
    {
        public static async Task<ExtendPagingResponse<T>> PagingAsync<T>(IPagingRequest request,
            IQueryable<T> data)
            where T : class
        {
            var rs = await PagingExtensions.PagingAsync(request, data);
            var response = new ExtendPagingResponse<T>(rs);
            response.SetTotal(await data.LongCountAsync());

            return response;
        }

        public static IQueryable<T> Filter<T, TPage>(
            this ExtendPagingResponse<TPage> paging, 
            IQueryable<T> data)
            where T : class
            where TPage : class
        {
            paging.SetTotal(data.LongCount());
            return PagingExtensions.Filter(paging, data);
        }
    }
}