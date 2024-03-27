namespace Core.Result
{
	public class PagingResponse<T> : PagingRequest where T : class
	{
		public IEnumerable<T> Data { get; internal set; }
		public int PageCount { get; internal set; }
		public int Total { get; internal set; }

        protected PagingResponse(IPagingRequest request)
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
        internal static PagingResponse<T> Result<TProto>(PagingResponse<TProto> request, IEnumerable<T> data) 
			where TProto: class
        {
			var response = new PagingResponse<T>(request)
			{
				Data = data,
				PageCount = request.PageCount,
				Total = request.Total
			};

			if (response.PageSize == 0 || response.PageIndex == 0)
				throw new NullReferenceException();
			if(data.Count() > response.PageSize)
				throw new ArgumentException();

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

            response.Total = data.Count();
            response.PageCount = (int)Math.Ceiling((decimal)response.Total / response.PageSize);
			response.Data = data.Skip(response.Skip).Take(response.PageSize);

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

			Total = data.Count();
			PageCount = (int)Math.Ceiling((decimal)Total / PageSize);

			return data.Skip(Skip).Take(PageSize);
		}
	}
}
