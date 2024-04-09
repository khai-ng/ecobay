namespace Core.SharedKernel
{
    public interface IAuditableEntity
    {
        public DateTimeOffset Created { get; }

        public string? CreatedBy { get; }

        public DateTimeOffset LastModified { get; }

        public string? LastModifiedBy { get; }
    }
}
