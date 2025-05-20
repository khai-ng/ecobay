namespace Core.Marten.Repository
{
	public class EventEntity(Guid id, object data, Type type, string typeName, long sequence, long version, DateTimeOffset createdAt)
	{
		public Guid Id { get; } = id;
		public object Data { get; } = data;
		public Type Type { get; } = type;
		public string TypeName { get; } = typeName;
		public long Sequence { get; } = sequence;
		public long Version { get; } = version;
		public DateTimeOffset CreatedAt { get; } = createdAt;
	}
}
