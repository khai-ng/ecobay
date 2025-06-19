namespace Ordering.API.Application.Ordering
{
    public record TrackingResponse(Guid Id, string TypeName, long Sequence, long Version, DateTimeOffset CreatedAt)
    { }
}
