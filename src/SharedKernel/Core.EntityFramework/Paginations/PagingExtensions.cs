using Core.Result.Paginations;
using Microsoft.EntityFrameworkCore;

namespace Core.EntityFramework.Paginations
{
    public static class PagingExtensions
    {
        public static async Task<PagingResponse<T>> PagingAsync<T>(
            this IPagingRequest request,
            IQueryable<T> data)
            where T : class
        {
            var response = new PagingResponse<T>(request);

            var filterData = await data
                .Skip(response.Skip)
                .Take(response.PageSize + 1)
                .ToListAsync();

            response.SetData(filterData
                .Take(response.PageSize));
            response.HasNext = response.PageSize < filterData.Count;

            return response;
        }

        /// <summary>
        /// Filter master collection
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static IQueryable<T> Filter<T, TPage>(this PagingResponse<TPage> paging, IQueryable<T> data)
            where T : class
            where TPage : class
        {
            paging.HasNext = data
                .Skip(paging.Skip + paging.PageSize)
                .Take(1)
                .Any();

            return data
                .Skip(paging.Skip)
                .Take(paging.PageSize);
        }
    }
}
