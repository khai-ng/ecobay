namespace Core.IntergrationEvent
{
    public interface IIntergrationEvent<TKey>
    {
        TKey Id { get; }
    }

    public interface IIntergrationEvent : IIntergrationEvent<Guid> { }
}
