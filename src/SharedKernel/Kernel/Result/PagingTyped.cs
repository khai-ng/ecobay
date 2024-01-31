namespace Infrastructure.Kernel.Result
{
	public class PagingTyped: PagingResponse<PagingTyped>
	{
		public PagingTyped() { }
		protected internal PagingTyped(PagingRequest request)
			: base(request) { }

		public static PagingTyped Load(PagingRequest request)
		{
			var response = new PagingTyped(request);
			return response;
		}	

		public PagingResponse<T> SetPagedData<T>(IEnumerable<T> data) where T : class
		{
			var response = new PagingResponse<T>();
			response.PageIndex = PageIndex;
			response.PageSize = PageSize;
			response.PageCount = PageCount;
			response.Total = Total;
			response.SetPagedData(data);

			return response;
		}

		public static PagingResponse<T> PagingResult<T>(IEnumerable<T> data, PagingRequest request)
			where T : class
		{
			var response = new PagingResponse<T>(request);
			return response.PagingResult(data);
		}
	}
}
