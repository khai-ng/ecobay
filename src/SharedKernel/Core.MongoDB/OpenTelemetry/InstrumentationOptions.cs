using MongoDB.Driver.Core.Events;

namespace Core.MongoDB.OpenTelemetry
{
    public class InstrumentationOptions
    {
        public bool CaptureCommandText { get; set; }
        public Func<CommandStartedEvent, bool> ShouldStartActivity { get; set; }
    }
}
