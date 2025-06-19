namespace Ordering.API.Application.Ordering
{
    public record TrackingCommand(Guid Id) : IRequest<AppResult<IEnumerable<TrackingResponse>>>
	{ }
}
