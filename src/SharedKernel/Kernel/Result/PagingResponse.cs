namespace SharedKernel.Kernel.Result
{

	public class PagingResponse<T> : PagingRequest where T : class
	{
		public IEnumerable<T> Data { get; internal set; }
		public int PageCount { get; internal set; }
		public int Total { get; internal set; }

		public PagingResponse() { }
		public PagingResponse(PagingRequest request)
		{
			SetRequest(request);
		}
		public void SetRequest(PagingRequest request)
		{
			PageSize = request.PageSize;
			PageIndex = request.PageIndex;
		}

		public void SetPagedData(IEnumerable<T> data)
		{
			if (PageSize == 0 || PageIndex == 0)
				throw new NullReferenceException();

			if(data.Count() > PageSize)
				throw new ArgumentException();

			Data = data;
		}

		/// <summary>
		/// Paging result data
		/// </summary>
		/// <param name="data"></param>
		/// <param name="request"></param>
		/// <returns></returns>
		public PagingResponse<T> PagingResult(IEnumerable<T> data, PagingRequest? request = null)
		{
			if(request is not null)
				SetRequest(request);

			if (PageSize == 0 || PageIndex == 0)
				throw new NullReferenceException();

			Total = data.Count();
			PageCount = (int)Math.Ceiling((decimal)Total / PageSize);
			Data = data.Skip(Skip).Take(PageSize);
			return this;
		}

		/// <summary>
		/// Paging master data without set paged data
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <param name="data"></param>
		/// <param name="request"></param>
		/// <returns></returns>
		public IQueryable<TEntity> PagingMaster<TEntity>(IQueryable<TEntity> data, PagingRequest? request = null) where TEntity : class
		{
			if (request is not null)
				SetRequest(request);

			if (PageSize == 0 || PageIndex == 0)
				throw new NullReferenceException();

			Total = data.Count();
			PageCount = (int)Math.Ceiling((decimal)Total / PageSize);

			return data.Skip(Skip).Take(PageSize);
		}
	}
}
