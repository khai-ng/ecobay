using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Core.Mediator.Tests
{
    public class RequestBaseHandlerTests
    {
        [Fact]
        public async Task Handle_WhenNoPipelines_ShouldReturnsHandledResult()
        {
            var services = new ServiceCollection();
            services.AddTransient<IRequestHandler<TestRequest, string>, TestHandler>();

            var provider = services.BuildServiceProvider();

            var sut = new RequestHandlerBase<TestRequest, string>(provider);

            var result = await sut.Handle(new TestRequest(), CancellationToken.None);

            Assert.Equal("handled", result);
        }

        [Fact]
        public async Task Handle_WithMultiplePipelines_ShouldExecutesPipelinesInOrder()
        {
            var services = new ServiceCollection();
            var calls = new List<string>();

            services.AddTransient<IRequestHandler<TestRequest, string>>(sp => new TestHandler(calls));
            services.AddTransient<IPipeline<TestRequest, string>>(sp => new RecordingPipeline(calls, "logging"));
            services.AddTransient<IPipeline<TestRequest, string>>(sp => new RecordingPipeline(calls, "validating"));

            var provider = services.BuildServiceProvider();

            var sut = new RequestHandlerBase<TestRequest, string>(provider);

            using var cts = new CancellationTokenSource();
            var token = cts.Token;

            var result = await sut.Handle(new TestRequest(), token);

            Assert.Equal("handled", result);

            Assert.Equal(new[] { "logging-before", "validating-before", "handler", "validating-after", "logging-after" }, calls);
        }

        private record TestRequest() : IRequest<string>;

        private class TestHandler : IRequestHandler<TestRequest, string>
        {
            private readonly List<string>? _calls;

            public TestHandler()
            {
            }

            public TestHandler(List<string> calls)
            {
                _calls = calls;
            }

            public Task<string> HandleAsync(TestRequest request, CancellationToken cancellationToken)
            {
                _calls?.Add("handler");
                return Task.FromResult("handled");
            }
        }

        private class RecordingPipeline : IPipeline<TestRequest, string>
        {
            private readonly List<string> _calls;
            private readonly string _name;

            public RecordingPipeline(List<string> calls, string name)
            {
                _calls = calls;
                _name = name;
            }

            public async Task<string> HandleAsync(TestRequest request, RequestHandlerDelegate<string> next, CancellationToken cancellationToken)
            {
                _calls.Add($"{_name}-before");
                var result = await next(cancellationToken);
                _calls.Add($"{_name}-after");
                return result;
            }
        }
    }
}
