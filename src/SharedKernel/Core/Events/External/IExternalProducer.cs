namespace Core.Events.External
{
    public interface IExternalProducer
    {
        Task PublishAsync<T>(T evt, CancellationToken cancellationToken = default)
            where T : IntergrationEvent;
    }
}
