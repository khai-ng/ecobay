﻿using Core.Pagination;
using Microsoft.EntityFrameworkCore;

namespace Core.EntityFramework.Pagination
{
    internal static class PagingExtensions
    {
        internal static async Task<PagingResponse<T>> PagingAsync<T>(
            IAllablePagingRequest request,
            IQueryable<T> data)
            where T : class
        {
            var response = new PagingResponse<T>(request);
            if (!request.GetAll ?? false)
                data = data.Skip(response.Skip).Take(response.PageSize + 1);

            var filterData = await data.ToListAsync().ConfigureAwait(false);
            response.SetData(filterData.Take(response.PageSize));
            response.SetHasNext(response.PageSize < filterData.Count);

            return response;
        }

        internal static IQueryable<T> FilterApply<T, TPage>(PagingResponse<TPage> paging, IQueryable<T> data)
            where T : class
            where TPage : class
        {
            paging.SetHasNext(
                !paging.GetAll ?? false
                && data
                .Skip(paging.Skip + paging.PageSize)
                .Any()
            );

            return paging.GetAll ?? false
                ? data
                : data.Skip(paging.Skip).Take(paging.PageSize);
        }
    }

    internal static class CountedPagingExtensions
    {
        internal static async Task<CountedPagingResponse<T>> PagingAsync<T>(IAllablePagingRequest request,
            IQueryable<T> data)
            where T : class
        {
            var rs = await PagingExtensions.PagingAsync(request, data).ConfigureAwait(false);
            var response = new CountedPagingResponse<T>(rs);
            response.SetTotal(data.LongCount());

            return response;
        }

        internal static IQueryable<T> FilterApply<T, TPage>(
            this CountedPagingResponse<TPage> paging,
            IQueryable<T> data)
            where T : class
            where TPage : class
        {
            paging.SetTotal(data.LongCount());
            return PagingExtensions.FilterApply(paging, data);
        }
    }
}
