using Core.Result.Paginations;
using MongoDB.Driver;

namespace Core.MongoDB.Paginations
{
    public static class PagingExtensions
    {
        internal static async Task<PagingResponse<TOut>> PagingAsync<TIn, TOut>(
            IPagingRequest request,
            IFindFluent<TIn, TOut> data)
            where TIn : class
            where TOut : class
        {
            var response = new PagingResponse<TOut>(request);

            var filterData = await data
                .Skip(response.Skip)
                .Limit(response.PageSize + 1)
                .ToListAsync();
            response.SetData(filterData
                .Take(response.PageSize));
            response.HasNext = response.PageSize < filterData.Count;

            return response;
        }

        /// <summary>
        /// Paging other collection. Set Total, PageCount of PagingResponse by this collection
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>

        public static IFindFluent<TIn, TOut> Filter<TIn, TOut, TPage>(
            this PagingResponse<TPage> response, 
            IFindFluent<TIn, TOut> data)
            where TIn : class
            where TOut : class
            where TPage : class
        {

            response.HasNext = data
                .Skip(response.Skip + response.PageSize)
                .Limit(1)
                .Any();

            return data
                .Skip(response.Skip)
                .Limit(response.PageSize);
        }
    }
}
