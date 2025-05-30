namespace Ordering.API.Application.Ordering
{
	public record TrackCommand : IRequest<AppResult<Guid>>
	{
	}

}
