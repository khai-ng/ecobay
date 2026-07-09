using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Core.Mediator.Tests
{
    public class MediatorTests
    {
        [Fact]
        public async Task Publish_CallsHandler()
        {
            var services = new ServiceCollection();
            services.AddTransient<IRequestHandler<TestRequest, string>, TestHandler>();
            var provider = services.BuildServiceProvider();

            var mediator = new Mediator(provider);
            var result = await mediator.Publish(new TestRequest(), CancellationToken.None);
            Assert.Equal("handled", result);
        }
    }

    internal class TestRequest: IRequest<string>
    {
    }

    internal class TestHandler : IRequestHandler<TestRequest, string>
    {

        Task<string> IRequestHandler<TestRequest, string>.Handle(TestRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult("handled");
        }
    }
}
