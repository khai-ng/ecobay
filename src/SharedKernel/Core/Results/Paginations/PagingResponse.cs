using Azure;
using Microsoft.EntityFrameworkCore;

namespace Core.Result.Paginations
{
    public class PagingResponse<T> : PagingRequest, IPagingResponse<T> where T : class
    {
        public IEnumerable<T> Data { get; protected set; }
        public bool HasNext { get; protected set; }

        protected PagingResponse(IPagingRequest request)
        {
            if (request.PageSize < 1 || request.PageIndex < 1)
                throw new NullReferenceException();

            PageSize = request.PageSize;
            PageIndex = request.PageIndex;
        }

        /// <summary>
        /// Set collection result
        /// </summary>
        /// <param name="data"></param>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="ArgumentException"></exception>
        internal static PagingResponse<T> Result<TProto>(IPagingResponse<TProto> request, 
            IEnumerable<T> data)
            where TProto : class
        {
            var response = new PagingResponse<T>(request)
            {
                Data = data,
                HasNext = request.HasNext,
            };
            return response;
        }

        internal static PagingResponse<T> Taking(IPagingRequest request, 
            IEnumerable<T> data)
        {
            var response = new PagingResponse<T>(request);
            IEnumerable<T> filterData = data
                .Take(response.PageSize + 1);
            response.Data = filterData
                .Take(response.PageSize);
            response.HasNext = response.PageSize < filterData.Count();

            return response;
        }

        /// <summary>
        /// Paging collection
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal static PagingResponse<T> Paging(IPagingRequest request, 
            IEnumerable<T> data)
        {
            var response = new PagingResponse<T>(request);

            var filterData = data
                .Skip(response.Skip)
                .Take(response.PageSize + 1);
            response.Data = filterData
                .Take(response.PageSize);
            response.HasNext = response.PageSize < filterData.Count();

            return response;
        }

        internal static async Task<PagingResponse<T>> PagingAsync(IPagingRequest request, 
            IQueryable<T> data)
        {
            var response = new PagingResponse<T>(request);

            var filterData = await data
                .Skip(response.Skip)
                .Take(response.PageSize + 1)
                .ToListAsync();
            response.Data = filterData
                .Take(response.PageSize);
            response.HasNext = response.PageSize < filterData.Count();

            return response;
        }

        /// <summary>
        /// Paging other collection. Set Total, PageCount of PagingResponse by this collection
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public IQueryable<TEntity> Filter<TEntity>(IQueryable<TEntity> data) 
            where TEntity : class
        {

            if (PageSize < 1 || PageIndex < 1)
                throw new NullReferenceException();

            HasNext = data
                .Skip(Skip + PageSize)
                .Take(1)
                .Any();

            return data
                .Skip(Skip)
                .Take(PageSize);
        }
    }
}
