using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

namespace Core.Result.Paginations
{
    public class PagingResponse<T> : PagingRequest, IPagingResponse<T> where T : class
    {
        public IEnumerable<T> Data { get; internal set; }
        //public long PageCount { get; internal set; }
        //public long Total { get; internal set; }
        public bool HasNext { get; internal set; }

        internal PagingResponse(IPagingRequest request)
        {
            PageSize = request.PageSize;
            PageIndex = request.PageIndex;
        }

        /// <summary>
        /// Set collection result
        /// </summary>
        /// <param name="data"></param>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="ArgumentException"></exception>
        internal static PagingResponse<T> Result<TProto>(IPagingResponse<TProto> request, IEnumerable<T> data)
            where TProto : class
        {
            var response = new PagingResponse<T>(request)
            {
                Data = data,
                HasNext = request.HasNext,
            };

            if (response.PageSize == 0 || response.PageIndex == 0)
                throw new NullReferenceException();

            return response;
        }

        /// <summary>
        /// Paging collection
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal static PagingResponse<T> Paging(IPagingRequest request, IEnumerable<T> data)
        {
            var response = new PagingResponse<T>(request);

            if (response.PageSize == 0 || response.PageIndex == 0)
                throw new NullReferenceException();

            IEnumerable<T> filterData = data.Skip(response.Skip).Take(response.PageSize + 1);

            response.Data = filterData.Take(response.PageSize);
            response.HasNext = response.PageSize < filterData.Count();

            return response;
        }

        internal static async Task<PagingResponse<T>> PagingAsync(IPagingRequest request, IQueryable<T> data)
        {
            var response = new PagingResponse<T>(request);

            if (response.PageSize == 0 || response.PageIndex == 0)
                throw new NullReferenceException();

            IEnumerable<T> filterData = await data.Skip(response.Skip).Take(response.PageSize + 1).ToListAsync();

            response.Data = filterData.Take(response.PageSize);
            response.HasNext = response.PageSize < filterData.Count();

            return response;
        }

        /// <summary>
        /// Paging other collection. Set Total, PageCount of PagingResponse by this collection
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public IQueryable<TEntity> Filter<TEntity>(IQueryable<TEntity> data) where TEntity : class
        {

            if (PageSize == 0 || PageIndex == 0)
                throw new NullReferenceException();

            //Total = data.LongCount();
            //PageCount = (long)Math.Ceiling((decimal)Total / PageSize);
            HasNext = data.Skip(Skip + PageSize).Take(1).Any();

            return data.Skip(Skip).Take(PageSize);
        }
    }
}
