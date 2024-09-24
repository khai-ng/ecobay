using Core.Result.Paginations;
using MongoDB.Driver;

namespace Core.MongoDB.Paginations
{
    public static class ExtendPagingExtensions
    {
        public static async Task<ExtendPagingResponse<T>> PagingAsync<T>(
            IPagingRequest request,
            IFindFluent<T, T> data)
            where T : class
        {
            var rs = await PagingExtensions.PagingAsync(request, data);
            var response = new ExtendPagingResponse<T>(rs);
            response.SetTotal(await data.CountDocumentsAsync());

            return response;
        }

        public static IFindFluent<TIn, TOut> Filter<TIn, TOut, TPage>(
            this ExtendPagingResponse<TPage> paging, 
            IFindFluent<TIn, TOut> data)
            where TIn : class
            where TOut : class
            where TPage : class
        {
            paging.SetTotal(data.CountDocuments());

            return PagingExtensions.Filter(paging, data);
        }
    }
}
