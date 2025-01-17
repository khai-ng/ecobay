using Core.Pagination;
using MongoDB.Driver;

namespace Core.MongoDB.Paginations
{
    internal static class PagingExtensions
    {
        internal static async Task<PagingResponse<TOut>> PagingAsync<TIn, TOut>(
            IPagingRequest request,
            IFindFluent<TIn, TOut> data)
            where TIn : class
            where TOut : class
        {
            var response = new PagingResponse<TOut>(request);
            if (!request.GetAll)
                data = data.Skip(response.Skip).Limit(response.PageSize + 1);

            var filterData = await data.ToListAsync().ConfigureAwait(false);
            response.SetData(filterData.Take(response.PageSize));
            response.HasNext = response.PageSize < filterData.Count;

            return response;
        }

        internal static IFindFluent<TIn, TOut> FilterApply<TIn, TOut, TPage>(
            this PagingResponse<TPage> paging, 
            IFindFluent<TIn, TOut> data)
            where TIn : class
            where TOut : class
            where TPage : class
        {
            paging.HasNext = paging.GetAll
                ? false
                : data.Skip(paging.Skip + paging.PageSize)
                .Limit(1).Any();

            return data
                .Skip(paging.Skip)
                .Limit(paging.PageSize);
        }
    }

    internal static class CountedPagingExtensions
    {
        internal static async Task<CountedPagingResponse<TOut>> PagingAsync<TIn, TOut>(
            IPagingRequest request,
            IFindFluent<TIn, TOut> data)
            where TIn : class
            where TOut : class
        {
            var rs = await PagingExtensions.PagingAsync(request, data).ConfigureAwait(false);
            var response = new CountedPagingResponse<TOut>(rs);
            response.Total(data.CountDocuments());

            return response;
        }

        internal static IFindFluent<TIn, TOut> FilterApply<TIn, TOut, TPage>(
            this CountedPagingResponse<TPage> paging,
            IFindFluent<TIn, TOut> data)
            where TIn : class
            where TOut : class
            where TPage : class
        {
            paging.Total(data.CountDocuments());
            return PagingExtensions.FilterApply(paging, data);
        }
    }
}
