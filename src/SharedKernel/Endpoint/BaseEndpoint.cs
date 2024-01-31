using Infrastructure.Kernel.Dependency;

namespace Infrastructure.Endpoint
{
    public interface IBaseEndpoint 
    {
        string Handler { get; }
    }


    public static class BaseEndpoint
    {
        public abstract class With<TResponse> : IBaseEndpoint
        {
            public string Handler => nameof(HandleAsync);

            public abstract Task<TResponse> HandleAsync(CancellationToken ct = default);

        }
        public abstract class With<TRequest, TResponse> : IBaseEndpoint
        {
            public string Handler => nameof(HandleAsync);

            public abstract Task<TResponse> HandleAsync(TRequest request,
                CancellationToken ct = default);

        }
    }
}
